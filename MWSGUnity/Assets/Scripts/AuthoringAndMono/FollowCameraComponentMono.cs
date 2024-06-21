using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class FollowCameraComponentMono : MonoBehaviour
	{
		public float3 Offset;

		public sealed class FollowCameraComponentBaker: Baker<FollowCameraComponentMono>
		{
			public override void Bake(FollowCameraComponentMono authoring)
			{
				var mainCamera = GameObject.FindWithTag("MainCamera");
				if (mainCamera != null)
				{
					AddComponentObject(GetEntity(TransformUsageFlags.Dynamic), new FollowCameraComponent{Offset = authoring.Offset, CameraTransform = mainCamera.transform});
				}
				else
				{
					Debug.Log("Cannot find MainCamera ");
				}
			}
		}
	}
}