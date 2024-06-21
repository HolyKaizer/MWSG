using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct MoveInput : IComponentData
	{
		public float2 Value;
	}
}