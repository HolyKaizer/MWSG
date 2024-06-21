using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
	[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
	public partial struct ResetInputSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var ecb = new EntityCommandBuffer(Allocator.Temp);
			foreach (var (tag, entity) in SystemAPI.Query<SimpleAttackInputComponent>().WithEntityAccess())
			{
				ecb.SetComponentEnabled<SimpleAttackInputComponent>(entity, false);
			}
			ecb.Playback(state.EntityManager);
			ecb.Dispose();
		}
	}
}