using Components;
using Components.InfoComponents;
using Components.Tags;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[BurstCompile]
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
			var moveJob = new PlayerMoveJob
			{
				DeltaTime = deltaTime,
				MoveInput = SystemAPI.GetSingleton<MoveInput>().Value
			}.Schedule(state.Dependency);
			state.Dependency = moveJob;
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
		}
	}
}