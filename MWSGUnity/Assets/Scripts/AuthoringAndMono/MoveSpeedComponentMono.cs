using Components;
using Components.InfoComponents;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class MoveSpeedComponentMono : MonoBehaviour
	{
		public float MoveSpeed;
	}

	public sealed class MoveSpeedComponentBaker : Baker<MoveSpeedComponentMono>
	{
		public override void Bake(MoveSpeedComponentMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.Dynamic), new MoveSpeedInfoComponent { Value = authoring.MoveSpeed});
		}
	}
}