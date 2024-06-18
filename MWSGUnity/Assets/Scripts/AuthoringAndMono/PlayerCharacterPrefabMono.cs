using Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class PlayerCharacterPrefabMono : MonoBehaviour
	{
		public GameObject PlayerCharacterPrefab;
		public class PlayerCharacterPrefabBaker : Baker<PlayerCharacterPrefabMono>
		{
			public override void Bake(PlayerCharacterPrefabMono authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponentObject(entity, new PlayerCharacterPrefab { Value = authoring.PlayerCharacterPrefab } );
			}
		}
	}
}