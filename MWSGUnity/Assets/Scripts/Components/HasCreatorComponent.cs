using Unity.Entities;

namespace Components
{
	public struct HasCreatorComponent : IComponentData
	{
		public Entity Creator;
	}
}