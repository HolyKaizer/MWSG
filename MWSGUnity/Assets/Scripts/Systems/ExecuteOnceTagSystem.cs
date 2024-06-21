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
			foreach (var (_, entity) in SystemAPI.Query<RefRO<ExecuteOnceTag>>().WithEntityAccess())
			{
				state.EntityManager.RemoveComponent<ExecuteOnceTag>(entity);
			}
		}
	}
}