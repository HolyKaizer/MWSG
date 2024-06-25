using Components;
using Components.Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial struct DestroyAfterTimeSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var deltaTime = SystemAPI.Time.DeltaTime;
			foreach (var (timeDataRef, isAliveRef) in SystemAPI.Query<RefRW<DestroyEntityAfterTimeComponent>, EnabledRefRW<DestroyEntityAfterTimeComponent>>())
			{
				if (timeDataRef.ValueRO.LifeTime > 0)
				{
					timeDataRef.ValueRW.LifeTime -= deltaTime;
				}
				else
				{
					isAliveRef.ValueRW = false;
				}
			}

			var entitiesToDestroy = SystemAPI.QueryBuilder().WithDisabled<DestroyEntityAfterTimeComponent>().Build();
			state.EntityManager.AddComponent<DestroyTag>(entitiesToDestroy);
		}
	}
}