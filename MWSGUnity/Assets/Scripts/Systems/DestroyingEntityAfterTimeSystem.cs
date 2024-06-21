using Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial struct DestroyAfterTimeSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BeginPresentationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var deltaTime = SystemAPI.Time.DeltaTime;
			var ecbSingleton = SystemAPI.GetSingleton<BeginPresentationEntityCommandBufferSystem.Singleton>();
			var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

			var jobHandle = new DestroyAfterTimeJob
			{
				DeltaTime = deltaTime,
				Ecb = ecb
			}.ScheduleParallel(state.Dependency);

			state.Dependency = jobHandle;
		}
	}
    
	[BurstCompile]
	public partial struct DestroyAfterTimeJob : IJobEntity
	{
		public float DeltaTime;
		public EntityCommandBuffer.ParallelWriter Ecb;

		private void Execute(Entity entity, [EntityIndexInQuery] int entityIndex, ref DestroyEntityAfterTimeComponent destroyAfterTime)
		{
			destroyAfterTime.TimeToLive -= DeltaTime;
			if (destroyAfterTime.TimeToLive <= 0)
			{
				Debug.Log($"Entity was destroyed entity.Index={entity.Index}, entity.Version={entity.Version}");
				Ecb.DestroyEntity(entityIndex, entity);
			}
		}
	}
}