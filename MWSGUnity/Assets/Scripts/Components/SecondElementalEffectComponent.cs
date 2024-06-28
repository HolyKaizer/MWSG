using Unity.Entities;

namespace Components
{
	public struct SecondElementalEffectComponent : IComponentData
	{
		public ElementalType Type;
		public Entity EntityThatApply;
	}
}