using Unity.Burst;
using Unity.Entities;

namespace Components
{
	[BurstCompile]
	public partial struct OrbManagementSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var jobHandle = new OrbManagementJob().ScheduleParallel(state.Dependency);
			state.Dependency = jobHandle;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{

		}
		
		[BurstCompile]
		public partial struct OrbManagementJob : IJobEntity
		{
			private void Execute(ref DynamicBuffer<OrbBufferElement> orbBuffer)
			{
			}
		}
	}
}