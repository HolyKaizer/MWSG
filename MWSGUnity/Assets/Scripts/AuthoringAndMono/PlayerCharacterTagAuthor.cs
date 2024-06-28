using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class PlayerCharacterTagAuthor : MonoBehaviour
	{
	}
	
	public sealed class PlayerCharacterTagBaker : Baker<PlayerCharacterTagAuthor>
	{
		public override void Bake(PlayerCharacterTagAuthor authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new PlayerCharacterTag());
		}
	}
}