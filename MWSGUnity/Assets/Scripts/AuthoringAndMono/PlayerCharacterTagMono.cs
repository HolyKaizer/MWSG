using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class PlayerCharacterTagMono : MonoBehaviour
	{
	}
	
	public sealed class PlayerCharacterTagBaker : Baker<PlayerCharacterTagMono>
	{
		public override void Bake(PlayerCharacterTagMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new PlayerCharacterTag());
		}
	}
}