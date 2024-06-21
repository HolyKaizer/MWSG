using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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
			var entity = GetEntity(authoring, TransformUsageFlags.None);
			AddComponent(entity, new SpawnPlayerCharacterComponent
			{
				PlayerCharacterPrefab = GetEntity(authoring.PlayerCharacterPrefab, TransformUsageFlags.Dynamic),
				SpawnScale = authoring.SpawnScale,
				SpawnPosition = authoring.SpawnPosition
			});
		}
	}
}