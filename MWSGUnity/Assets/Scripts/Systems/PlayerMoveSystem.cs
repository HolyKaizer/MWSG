using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
	[UpdateBefore(typeof(TransformSystemGroup))]
	public partial struct PlayerMoveSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<MoveInput>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var deltaTime = SystemAPI.Time.DeltaTime;
			var jobHandle = new PlayerMoveJob
			{
				DeltaTime = deltaTime,
				MoveInput = SystemAPI.GetSingleton<MoveInput>().Value
			}.ScheduleParallel(state.Dependency);

			state.Dependency = jobHandle;
		}
	}
	
	[BurstCompile]
	public partial struct PlayerMoveJob : IJobEntity
	{
		public float DeltaTime;
		public float2 MoveInput;

		private void Execute(ref LocalTransform transform, in MoveSpeedComponent moveSpeedComponent, in PlayerCharacterTagComponent tagComponent)
		{
			transform.Position.xz += MoveInput * moveSpeedComponent.Value * DeltaTime;
			if (math.lengthsq(MoveInput) > float.Epsilon)
			{
				var forward = new float3(MoveInput.x, 0, MoveInput.y);
				transform.Rotation = quaternion.LookRotation(forward, math.up());
				Debug.Log($"Move by {MoveInput}, cur pos = {transform.Position},  cur rot={transform.Rotation.value}, tagComponent={tagComponent}");
			}
		}
	}
}