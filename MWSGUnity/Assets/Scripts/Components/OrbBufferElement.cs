using Unity.Entities;

namespace Components
{
	[InternalBufferCapacity(4)]
	public struct OrbBufferElement : IBufferElementData
	{
		public OrbRuntimeData RuntimeData;
	}

	public struct OrbRuntimeData
	{
		public ElementalType Type;
	}
}