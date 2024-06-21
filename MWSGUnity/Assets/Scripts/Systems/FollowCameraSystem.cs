using Components;
using Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial class FollowCameraSystem : SystemBase
	{
		private EntityQuery followCameraQuery;
		protected override void OnCreate()
		{
			base.OnCreate();
			followCameraQuery = GetEntityQuery(ComponentType.ReadOnly<FollowCameraComponent>());
		}

		protected override void OnUpdate()
		{
			if (followCameraQuery.CalculateEntityCount() <= 0) return;
			
			var followCameraEntity = followCameraQuery.ToEntityArray(Allocator.Temp)[0];
			var followCameraComponent = EntityManager.GetComponentObject<FollowCameraComponent>(followCameraEntity);
			foreach (var (transform, _) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerCharacterTag>>())
			{
				followCameraComponent.CameraTransform.position = transform.ValueRO.Position + followCameraComponent.Offset;
				break;
			}
		}
	}
}