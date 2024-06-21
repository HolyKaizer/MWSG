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
				if (!state.EntityManager.Exists(spawnComponent.ValueRO.PlayerCharacterPrefab))
				{
					Debug.LogError($"Doensnt't exists enityty: {spawnComponent.ValueRO.PlayerCharacterPrefab}");
				}
				var entity = state.EntityManager.Instantiate(spawnComponent.ValueRO.PlayerCharacterPrefab);
				state.EntityManager.SetComponentData(entity, new LocalTransform
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