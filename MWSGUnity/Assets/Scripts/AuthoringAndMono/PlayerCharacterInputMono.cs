using Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class PlayerCharacterInputMono : MonoBehaviour
	{
	}

	public class PlayerCharacterInputMonoBaker : Baker<PlayerCharacterInputMono>
	{
		public override void Bake(PlayerCharacterInputMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new PlayerCharacterMoveInput());
		}
	}
}