using Components;
using Components.InfoComponents;
using Components.Tags;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
			new PlayerMoveJob
			{
				DeltaTime = deltaTime,
				MoveInput = SystemAPI.GetSingleton<MoveInput>().Value
			}.Schedule();
		}
	}
	
	[BurstCompile]
	public partial struct PlayerMoveJob : IJobEntity
	{
		public float DeltaTime;
		public float2 MoveInput;

		private void Execute(ref LocalTransform transform, in MoveSpeedInfoComponent moveSpeedInfoComponent, in PlayerCharacterTag tag)
		{
			transform.Position.xz += MoveInput * moveSpeedInfoComponent.Value * DeltaTime;
			if (math.lengthsq(MoveInput) > float.Epsilon)
			{
				var forward = new float3(MoveInput.x, 0, MoveInput.y);
				transform.Rotation = quaternion.LookRotation(forward, math.up());
			}
		}
	}
}