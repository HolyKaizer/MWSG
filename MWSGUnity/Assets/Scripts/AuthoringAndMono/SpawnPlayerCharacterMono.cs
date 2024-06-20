using Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class SpawnPlayerCharacterMono : MonoBehaviour
	{
		public GameObject PlayerCharacterPrefab;
	}

	public sealed class SpawnPlayerCharacterBaker : Baker<SpawnPlayerCharacterMono>
	{
		public override void Bake(SpawnPlayerCharacterMono authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.None);
			AddComponent(entity, new SpawnPlayerCharacterComponent
			{
				PlayerCharacterPrefab = GetEntity(authoring.PlayerCharacterPrefab, TransformUsageFlags.Dynamic)
			});
		}
	}
}