using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
	public struct PlayerCharacterMoveInput : IComponentData
	{
		public float2 Value;
	}
	
	public struct PlayerCharacterMoveSpeed : IComponentData
	{
		public float Value;
	}
}