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
			state.RequireForUpdate<PlayerCharacterTag>();
			state.RequireForUpdate<ElementalAttackCollectionInfoComponent>();
			state.RequireForUpdate<OrbBufferElement>();
		}
		[BurstDiscard]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var _ in SystemAPI.Query<RefRO<SimpleAttackInputComponent>>())
			{
				var player = SystemAPI.GetSingletonEntity<PlayerCharacterTag>();
				var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(player);
				var orbBuffer = SystemAPI.GetBuffer<OrbBufferElement>(player);
				if(orbBuffer.Length <= 0) return;
				var testComponent = SystemAPI.GetComponent<TestSpawnProjComponent>(SystemAPI.GetSingletonEntity<ElementalAttackCollectionInfoComponent>());
				ref var info = ref FindElementInfo(orbBuffer);
				
				if (!state.EntityManager.Exists(testComponent.TestSpawnPRefab))
				{
					Debug.LogError($"Doensnt't exists testComponent.TestSpawnPRefab: {testComponent.TestSpawnPRefab}");
				}
				
				if (!state.EntityManager.Exists(info.EntityPrefab.Value))
				{
					Debug.LogError($"Doensnt't exists info.EntityPrefab: {info.EntityPrefab}");
				}
				
				var newProjectileEntity = state.EntityManager.Instantiate(info.EntityPrefab.Value);

				/*
				var projectileTransform = LocalTransform.FromPositionRotationScale(playerTransform.ValueRO.Position, playerTransform.ValueRO.Rotation, 1f);
				var movingComponent = new MovingInDirectionComponent { CurrentSpeed = info.Speed, Direction = projectileTransform.Forward() }; 
				var destroyEntityAfterTime = new DestroyEntityAfterTimeComponent { TimeToLive = info.TravelTime};
				var damageDealerComponent = new DamageDealerComponent { Value = info.DamageAmount };
				
				state.EntityManager.SetComponent(newProjectileEntity, projectileTransform);
				state.EntityManager.AddComponent(newProjectileEntity, movingComponent);
				state.EntityManager.AddComponent(newProjectileEntity, damageDealerComponent);
				state.EntityManager.AddComponent(newProjectileEntity, destroyEntityAfterTime);
				*/
				Debug.Log($"newProjectile.Index={newProjectileEntity.Index} newProjectile.Version={newProjectileEntity.Version}, info.EntityPrefab.Index={info.EntityPrefab.Value.Index}, info.EntityPrefab.Version={info.EntityPrefab.Value.Version}");
			}
		}

		private ref ElementalAttackItemInfo FindElementInfo(DynamicBuffer<OrbBufferElement> orbBuffer)
		{
			ref var elements = ref SystemAPI.GetSingleton<ElementalAttackCollectionInfoComponent>().InfosReference.Value.ElementsInfo;
			Debug.Log($"Blob array lntg is: {elements.Length}");
			for (var i = 0; i < elements.Length; i++)
			{
				Debug.Log($"Entitiy in blobArray is: {elements[i].EntityPrefab.Value.ToFixedString()}");
			}
			for (var i = 0; i < elements.Length; i++)
			{
				if (elements[i].Type == orbBuffer[0].RuntimeData.Type) 
					return ref elements[i];
			}
			return ref elements[0];
		}
	}
}