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
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<ExecuteOnceTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var ecb = SystemAPI
				.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(state.WorldUnmanaged);
			foreach (var (_, entity) in SystemAPI.Query<RefRO<ExecuteOnceTag>>().WithEntityAccess())
			{
				ecb.RemoveComponent<ExecuteOnceTag>(entity);
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{

		}
	}
}