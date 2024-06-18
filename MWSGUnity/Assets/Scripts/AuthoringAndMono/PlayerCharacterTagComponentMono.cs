using Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class PlayerCharacterTagComponentMono : MonoBehaviour
	{
		public float MoveSpeed;
	}

	public class PlayerCharacterTagComponentBaker : Baker<PlayerCharacterTagComponentMono>
	{
		public override void Bake(PlayerCharacterTagComponentMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new PlayerCharacterTagComponent());
			AddComponent(GetEntity(authoring, TransformUsageFlags.Dynamic), new PlayerCharacterMoveSpeed { Value = authoring.MoveSpeed});
		}
	}
}