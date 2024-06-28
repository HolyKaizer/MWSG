using Components;
using Components.InfoComponents;
using Components.Tags;
using Components.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	[UpdateBefore(typeof(TransformSystemGroup) )]
	public partial struct PlayerSimpleAttackSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<CursorSelectionComponent>();
			state.RequireForUpdate<EntityPrefabRegisterSystem.Map>();
			state.RequireForUpdate<PlayerCharacterTag>();
			state.RequireForUpdate<ElementalAttackCollectionInfoComponent>();
			state.RequireForUpdate<OrbBufferElement>();
		}
		
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.QueryBuilder().WithAll<SimpleAttackInputComponent>().Build().IsEmpty) return;
			
			using var ecb = new EntityCommandBuffer(Allocator.Temp); 
			var prefabMap = SystemAPI.GetSingleton<EntityPrefabRegisterSystem.Map>();
			var player = SystemAPI.GetSingletonEntity<PlayerCharacterTag>();
			var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(player);
			var cursorLocalTransform = SystemAPI.GetSingleton<CursorSelectionComponent>();
			var orbBuffer = SystemAPI.GetBuffer<OrbBufferElement>(player);
			if(orbBuffer.Length <= 0) return;
			
			ref var info = ref FindElementInfo(ref orbBuffer);
			var entityPrefab = info.EntityPrefab.GetEntity(prefabMap);
			var newProjectileEntity = state.EntityManager.Instantiate(entityPrefab);

			var direction = math.normalize(cursorLocalTransform.CurrentGroundPoint - playerTransform.ValueRO.Position);
			direction.y = 0;
			var rotation = quaternion.LookRotation(direction, math.up());
			var projectileTransform = LocalTransform.FromPositionRotationScale(playerTransform.ValueRO.Position, rotation, 1f);
			var movingComponent = new MovingInDirectionComponent { CurrentSpeed = info.Speed, Direction = direction }; 
			var destroyEntityAfterTime = new DestroyEntityAfterTimeComponent { LifeTime = info.TravelTime};
			var damageDealerComponent = new DamageDealerComponent { Value = info.DamageAmount };
			var hasCreatorComponent = new HasCreatorComponent { Creator = player };

			ecb.SetComponent(newProjectileEntity, projectileTransform);
			ecb.AddComponent(newProjectileEntity, movingComponent);
			ecb.AddComponent(newProjectileEntity, damageDealerComponent);
			ecb.AddComponent(newProjectileEntity, destroyEntityAfterTime);
			ecb.AddComponent(newProjectileEntity, hasCreatorComponent);
			RotateOrbBuffer(ref orbBuffer);
			
			ecb.Playback(state.EntityManager);
		}
		
		[BurstCompile]
		private void RotateOrbBuffer(ref DynamicBuffer<OrbBufferElement> orbBuffer)
		{
			if (orbBuffer.Length <= 1) return;
			var firstElement = orbBuffer[0];
			for (int i = 1; i < orbBuffer.Length; i++)
			{
				orbBuffer[i - 1] = orbBuffer[i];
			}
			orbBuffer[^1] = firstElement;
		}
		
		[BurstCompile]
		private ref ElementalAttackItemInfo FindElementInfo(ref DynamicBuffer<OrbBufferElement> orbBuffer)
		{
			ref var elements = ref SystemAPI.GetSingleton<ElementalAttackCollectionInfoComponent>().InfosReference.Value.ElementsInfo;
			for (var i = 0; i < elements.Length; i++)
			{
				if (elements[i].Type == orbBuffer[0].RuntimeData.Type) 
					return ref elements[i];
			}
			return ref elements[0];
		}
	}
}