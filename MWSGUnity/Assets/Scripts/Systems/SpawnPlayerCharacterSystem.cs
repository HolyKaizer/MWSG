using Components;
using Components.Tags;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	[UpdateBefore(typeof(ExecuteOnceTagSystem))]
	public partial struct SpawnPlayerCharacterSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
		}
		
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var (_, spawnComponent) in SystemAPI.Query<RefRO<ExecuteOnceTag>, RefRO<SpawnPlayerCharacterComponent>>())
			{
				var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
				var entity = ecb.Instantiate(spawnComponent.ValueRO.PlayerCharacterPrefab);
				ecb.SetComponent(entity, new LocalTransform
				{
					Position = spawnComponent.ValueRO.SpawnPosition,
					Rotation = quaternion.identity,
					Scale = spawnComponent.ValueRO.SpawnScale
				});
				Debug.Log($"There's player instantiate:entity.Index={entity.Index} entity.Version={entity.Version} entity.GetHashCode={entity.GetHashCode()}");
			}
		}
	} 
}