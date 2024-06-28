using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class SpawnPlayerCharacterAuthor : MonoBehaviour
	{
		public GameObject PlayerCharacterPrefab;
		public float3 SpawnPosition;
		public float SpawnScale;
	}

	public sealed class SpawnPlayerCharacterBaker : Baker<SpawnPlayerCharacterAuthor>
	{
		public override void Bake(SpawnPlayerCharacterAuthor authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new SpawnPlayerCharacterComponent
			{
				PlayerCharacterPrefab = GetEntity(authoring.PlayerCharacterPrefab, TransformUsageFlags.Dynamic),
				SpawnScale = authoring.SpawnScale,
				SpawnPosition = authoring.SpawnPosition
			});
		}
	}
}