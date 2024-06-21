using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct SpawnPlayerCharacterComponent : IComponentData
	{
		public float3 SpawnPosition;
		public float SpawnScale;
		public Entity PlayerCharacterPrefab;
	}
}