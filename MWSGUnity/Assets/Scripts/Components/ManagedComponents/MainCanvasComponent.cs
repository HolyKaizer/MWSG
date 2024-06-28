using Unity.Entities;
using UnityEngine;

namespace Components.ManagedComponents
{
	public class MainCanvasComponent : IComponentData
	{
		public Canvas RootCanvas;
		public RectTransform CursorRect;
	}
}