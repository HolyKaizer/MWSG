using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct BattleCoreRandomComponent : IComponentData
	{
		public Random Value;
	}
}