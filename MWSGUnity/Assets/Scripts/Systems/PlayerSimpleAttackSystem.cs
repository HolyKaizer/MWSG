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
		[BurstDiscard]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PlayerCharacterTag>();
			state.RequireForUpdate<ElementalAttackCollectionInfoComponent>();
			state.RequireForUpdate<OrbBufferElement>();
		}
		[BurstDiscard]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var _ in SystemAPI.Query<RefRO<SimpleAttackInputComponent>>())
			{
				var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
				var player = SystemAPI.GetSingletonEntity<PlayerCharacterTag>();
				var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(player);
				var orbBuffer = SystemAPI.GetBuffer<OrbBufferElement>(player);
				if(orbBuffer.Length <= 0) return;
				var testComponent = SystemAPI.GetComponent<TestSpawnProjComponent>(SystemAPI.GetSingletonEntity<ElementalAttackCollectionInfoComponent>());
				ref var info = ref FindElementInfo(orbBuffer);
				var projectileTransform = LocalTransform.FromPositionRotationScale(playerTransform.ValueRO.Position, playerTransform.ValueRO.Rotation, 1f);
				var movingComponent = new MovingInDirectionComponent { CurrentSpeed = info.Speed, Direction = projectileTransform.Forward() }; 
				var destroyEntityAfterTime = new DestroyEntityAfterTimeComponent { TimeToLive = info.TravelTime};
				
				var newProjectileEntity = ecb.Instantiate(info.EntityPrefab);
				Debug.Log($"newProjectile.Index={newProjectileEntity.Index} newProjectile.Version={newProjectileEntity.Version}, info.EntityPrefab.Index={info.EntityPrefab.Index}, info.EntityPrefab.Version={info.EntityPrefab.Version}");
			}
		}

		private ref ElementalAttackItemInfo FindElementInfo(DynamicBuffer<OrbBufferElement> orbBuffer)
		{
			var reference = SystemAPI.GetSingleton<ElementalAttackCollectionInfoComponent>().InfosReference;
			for (var i = 0; i < reference.Value.ElementsInfo.Length; i++)
			{
				if (reference.Value.ElementsInfo[i].Type == orbBuffer[0].RuntimeData.Type) 
					return ref reference.Value.ElementsInfo[i];
			}
			return ref reference.Value.ElementsInfo[0];
		}
	}
}