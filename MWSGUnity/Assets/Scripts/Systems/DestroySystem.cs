using Components.Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial struct DestroySystem : ISystem
	{
		private EntityQuery _query;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_query = state.GetEntityQuery(ComponentType.ReadOnly<DestroyTag>());
		}
		
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var entityList = new NativeList<Entity>(Allocator.Temp);

			var linkedLookup = SystemAPI.GetBufferLookup<LinkedEntityGroup>();
			var linkedEntitiesToDestroy = SystemAPI.QueryBuilder().WithAll<DestroyTag, LinkedEntityGroup>().Build().ToEntityArray(Allocator.Temp);

			foreach (var linkedEntity in linkedEntitiesToDestroy)
			{
				entityList.AddRange(linkedLookup[linkedEntity].AsNativeArray().Reinterpret<Entity>());
			}

			state.EntityManager.AddComponent<DestroyTag>(entityList.AsArray());
			state.EntityManager.DestroyEntity(_query);
		}
	}
}