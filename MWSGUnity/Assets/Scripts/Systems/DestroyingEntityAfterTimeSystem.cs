using Components;
using Components.Tags;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
	[BurstCompile]
	[UpdateBefore(typeof(DestroySystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct DestroyAfterTimeSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var deltaTime = SystemAPI.Time.DeltaTime;
			new CheckToDestroyJob
			{
				DeltaTime = deltaTime
			}.Run();
			
			var entitiesToDestroy = SystemAPI.QueryBuilder().WithDisabled<DestroyEntityAfterTimeComponent>().Build();
			state.EntityManager.AddComponent<DestroyTag>(entitiesToDestroy);
		}
	}

	[BurstCompile]
	public partial struct CheckToDestroyJob : IJobEntity
	{
		public float DeltaTime;
		
		private void Execute(ref DestroyEntityAfterTimeComponent timeDataRef, EnabledRefRW<DestroyEntityAfterTimeComponent> isAliveRef)
		{
			if (timeDataRef.LifeTime > 0)
			{
				timeDataRef.LifeTime -= DeltaTime;
			}
			else
			{
				isAliveRef.ValueRW = false;
			}
		}
	}
}