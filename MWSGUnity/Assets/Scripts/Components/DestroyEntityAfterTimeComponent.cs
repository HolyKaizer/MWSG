using Unity.Entities;

namespace Components
{
	public struct DestroyEntityAfterTimeComponent : IComponentData, IEnableableComponent
	{
		public float LifeTime;
	}
}