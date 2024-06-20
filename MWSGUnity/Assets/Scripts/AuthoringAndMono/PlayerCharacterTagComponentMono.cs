using Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class PlayerCharacterTagComponentMono : MonoBehaviour
	{
	}
	
	public sealed class PlayerCharacterTagComponentBaker : Baker<PlayerCharacterTagComponentMono>
	{
		public override void Bake(PlayerCharacterTagComponentMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new PlayerCharacterTagComponent());
		}
	}
}