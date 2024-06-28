using Components.Tags;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial struct ExecuteOnceTagSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.EntityManager.RemoveComponent<ExecuteOnceTag>(SystemAPI.QueryBuilder().WithAll<ExecuteOnceTag>().Build());
		}
	}
}