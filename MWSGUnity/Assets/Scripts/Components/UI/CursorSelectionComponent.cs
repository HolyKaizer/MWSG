using Unity.Entities;
using Unity.Mathematics;

namespace Components.UI
{
	public enum CursorToDraw
	{
		Default,
		SelectOnObject
	}
	public struct CursorSelectionComponent : IComponentData
	{
		public CursorToDraw CursorToDrawType;
		public Entity HoveredEntity;
		public float3 CurrentGroundPoint;
	}
}