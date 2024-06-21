using Unity.Entities;

namespace Components
{
	public struct DestroyEntityAfterTimeComponent : IComponentData
	{
		public float TimeToLive;
	}
}