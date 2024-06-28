using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components.ManagedComponents
{
	public class FollowCameraComponent : IComponentData
	{
		public Transform CameraTransform;
		public Camera Camera;
		public Camera UICamera;
		public float3 Offset;
	}
}