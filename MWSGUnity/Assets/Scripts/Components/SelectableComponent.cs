using Unity.Entities;

namespace Components
{
	public struct SelectableComponent : IComponentData, IEnableableComponent
	{
		public Entity OutlineEntity;
	}
}