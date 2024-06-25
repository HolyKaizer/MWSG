using Components;
using Components.InfoComponents;
using Components.Tags;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	[UpdateBefore(typeof(TransformSystemGroup) )]
	public partial struct PlayerSimpleAttackSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<EntityPrefabRegisterSystem.Map>();
			state.RequireForUpdate<PlayerCharacterTag>();
			state.RequireForUpdate<ElementalAttackCollectionInfoComponent>();
			state.RequireForUpdate<OrbBufferElement>();
		}
		
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.QueryBuilder().WithAll<SimpleAttackInputComponent>().Build().IsEmpty) return;
			
			var prefabMap = SystemAPI.GetSingleton<EntityPrefabRegisterSystem.Map>();
			var player = SystemAPI.GetSingletonEntity<PlayerCharacterTag>();
			var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(player);
			var orbBuffer = SystemAPI.GetBuffer<OrbBufferElement>(player);
			if(orbBuffer.Length <= 0) return;
				
			ref var info = ref FindElementInfo(orbBuffer);
			var entityPrefab = info.EntityPrefab.GetEntity(prefabMap);
			var newProjectileEntity = state.EntityManager.Instantiate(entityPrefab);

			var projectileTransform = LocalTransform.FromPositionRotationScale(playerTransform.ValueRO.Position, playerTransform.ValueRO.Rotation, 1f);
			var movingComponent = new MovingInDirectionComponent { CurrentSpeed = info.Speed, Direction = projectileTransform.Forward() }; 
			var destroyEntityAfterTime = new DestroyEntityAfterTimeComponent { LifeTime = info.TravelTime};
			var damageDealerComponent = new DamageDealerComponent { Value = info.DamageAmount };
				
			state.EntityManager.SetComponentData(newProjectileEntity, projectileTransform);
			state.EntityManager.AddComponentData(newProjectileEntity, movingComponent);
			state.EntityManager.AddComponentData(newProjectileEntity, damageDealerComponent);
			state.EntityManager.AddComponentData(newProjectileEntity, destroyEntityAfterTime);
				
			Debug.Log($"newProjectile.Index={newProjectileEntity.Index} newProjectile.Version={newProjectileEntity.Version}, entityPrefab.Index={entityPrefab.Index}, entityPrefab.Version={entityPrefab.Version}");
		}
		
		[BurstCompile]
		private ref ElementalAttackItemInfo FindElementInfo(DynamicBuffer<OrbBufferElement> orbBuffer)
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