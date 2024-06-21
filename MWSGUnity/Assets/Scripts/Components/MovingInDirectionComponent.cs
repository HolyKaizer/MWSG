using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct MovingInDirectionComponent : IComponentData
	{
		public float3 Direction;
		public float CurrentSpeed;
	}
}