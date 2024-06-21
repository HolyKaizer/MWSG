using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class SpawnPlayerCharacterMono : MonoBehaviour
	{
		public GameObject PlayerCharacterPrefab;
		public float3 SpawnPosition;
		public float SpawnScale;
	}

	public sealed class SpawnPlayerCharacterBaker : Baker<SpawnPlayerCharacterMono>
	{
		public override void Bake(SpawnPlayerCharacterMono authoring)
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