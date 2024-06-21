using Unity.Entities;

namespace Components.InfoComponents
{
	public struct ElementalAttackItemInfo
	{
		public ElementalType Type;
		public float TravelTime;
		public float Speed;
		public Entity EntityPrefab;
	}
}