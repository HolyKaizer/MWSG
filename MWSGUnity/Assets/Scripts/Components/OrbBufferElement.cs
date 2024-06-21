using Unity.Entities;

namespace Components
{
	public struct OrbBufferElement : IBufferElementData
	{
		public OrbRuntimeData RuntimeData;
	}

	public struct OrbRuntimeData
	{
		public ElementalType Type;
	}
}