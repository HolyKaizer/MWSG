using Unity.Entities;

namespace Components
{
	public struct SpawnPlayerCharacterComponent : IComponentData
	{
		public Entity PlayerCharacterPrefab;
	}
}