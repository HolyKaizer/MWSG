using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
	public class FollowCameraComponent : IComponentData
	{
		public Transform CameraTransform;
		public float3 Offset;
	}
}