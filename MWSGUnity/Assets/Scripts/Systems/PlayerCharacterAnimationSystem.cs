using Components;
using Rukhanka;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct PlayerCharacterAnimationSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<MoveInput>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var moveInput = SystemAPI.GetSingleton<MoveInput>();
			
			foreach (var animator  in SystemAPI.Query<DynamicBuffer<AnimatorControllerParameterComponent>>())
			{
				var isMovingParameter = animator[0];
				var isGroundedParameter = animator[1];
				isMovingParameter.BoolValue = math.lengthsq(moveInput.Value) > 0;
				isGroundedParameter.BoolValue = true;
				animator.ElementAt(0) = isMovingParameter;
				animator.ElementAt(1) = isGroundedParameter;
			}
		}
	}
}