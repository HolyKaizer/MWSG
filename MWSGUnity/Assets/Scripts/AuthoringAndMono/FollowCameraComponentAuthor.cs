using Components.ManagedComponents;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class FollowCameraComponentAuthor : MonoBehaviour
	{
		public float3 Offset;

		public sealed class FollowCameraComponentBaker: Baker<FollowCameraComponentAuthor>
		{
			public override void Bake(FollowCameraComponentAuthor authoring)
			{
				var mainCamera = GameObject.FindWithTag("MainCamera");
				
				if (mainCamera != null)
				{
					AddComponentObject(GetEntity(TransformUsageFlags.Dynamic), new FollowCameraComponent{Offset = authoring.Offset, CameraTransform = mainCamera.transform, Camera = mainCamera.GetComponent<Camera>(), UICamera = mainCamera.GetComponentInChildren<Camera>()});
				}
				else
				{
					Debug.Log("Cannot find MainCamera ");
				}
			}
		}
	}
}