using Components.ManagedComponents;
using Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial struct FollowCameraSystem : ISystem
	{
		private EntityQuery followCameraQuery;

		public void OnCreate(ref SystemState state)
		{
			followCameraQuery = state.GetEntityQuery(ComponentType.ReadOnly<FollowCameraComponent>());
		}

		public void OnUpdate(ref SystemState state)
		{
			if (followCameraQuery.CalculateEntityCount() <= 0) return;
			
			var followCameraEntity = followCameraQuery.ToEntityArray(Allocator.Temp)[0];
			var followCameraComponent = SystemAPI.ManagedAPI.GetComponent<FollowCameraComponent>(followCameraEntity);
			foreach (var (transform, _) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerCharacterTag>>())
			{
				followCameraComponent.CameraTransform.position = transform.ValueRO.Position + followCameraComponent.Offset;
				break;
			}
		}
	}
}