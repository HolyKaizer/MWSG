using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial struct MovingInDirectionSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var deltaTime = SystemAPI.Time.DeltaTime;

			var jobHandle = new MoveJob
			{
				DeltaTime = deltaTime
			}.ScheduleParallel(state.Dependency);

			state.Dependency = jobHandle;
		}
	}
    
	[BurstCompile]
	public partial struct MoveJob : IJobEntity
	{
		public float DeltaTime;

		private void Execute(ref LocalTransform transform, in MovingInDirectionComponent moveComponent)
		{
			transform.Position += moveComponent.Direction * (moveComponent.CurrentSpeed * DeltaTime);
		}
	}
}