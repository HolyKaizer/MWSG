using Unity.Entities;

namespace Components
{
	public struct FirstElementalEffectComponent : IComponentData
	{
		public ElementalType Type;
		public Entity EntityThatApply;
	}
}