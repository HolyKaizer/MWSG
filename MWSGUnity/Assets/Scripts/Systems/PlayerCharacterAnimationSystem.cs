using Components;
using Components.UI;
using Rukhanka;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[BurstCompile]
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
			foreach (var (animator, entity)  in SystemAPI.Query<DynamicBuffer<AnimatorControllerParameterComponent>>().WithEntityAccess())
			{
				var playerTransform = state.EntityManager.GetComponentData<LocalTransform>(entity);
				var isMovingParameter = animator[0];
				var isGroundedParameter = animator[1];

				isMovingParameter.BoolValue = math.lengthsq(moveInput.Value) > 0;
				
				var xParameter = animator[4];
				var yParameter = animator[5];
				var isShootingParameter = animator[6];
				var weaponTypeParameter = animator[9];
				var aimDirection = playerTransform.InverseTransformDirection(playerTransform.Forward());
				yParameter.FloatValue = aimDirection.z;
				xParameter.FloatValue = aimDirection.x;
				isShootingParameter.BoolValue = true;
				isGroundedParameter.BoolValue = true;
				weaponTypeParameter.IntValue = 2;
				playerTransform.Rotate(quaternion.LookRotation(aimDirection, math.up()));
				animator.ElementAt(0) = isMovingParameter;
				animator.ElementAt(1) = isGroundedParameter;
				animator.ElementAt(4) = xParameter;
				animator.ElementAt(5) = yParameter;
				animator.ElementAt(6) = isShootingParameter;
				animator.ElementAt(9) = weaponTypeParameter;
			}
		}
	}
}