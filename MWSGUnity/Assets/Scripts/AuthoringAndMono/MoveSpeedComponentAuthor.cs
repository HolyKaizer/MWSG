using Components.InfoComponents;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class MoveSpeedComponentAuthor : MonoBehaviour
	{
		public float MoveSpeed;
	}

	public sealed class MoveSpeedComponentBaker : Baker<MoveSpeedComponentAuthor>
	{
		public override void Bake(MoveSpeedComponentAuthor authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.Dynamic), new MoveSpeedInfoComponent { Value = authoring.MoveSpeed});
		}
	}
}