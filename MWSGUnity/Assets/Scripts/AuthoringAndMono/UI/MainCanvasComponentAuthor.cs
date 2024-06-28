using AuthoringAndMono.MonoScripts;
using Components.ManagedComponents;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono.UI
{
	public sealed class MainCanvasComponentAuthor : MonoBehaviour
	{
		public sealed class MainCanvasComponentBaker: Baker<MainCanvasComponentAuthor>
		{
			public override void Bake(MainCanvasComponentAuthor authoring)
			{
				var mainCanvas = GameObject.FindWithTag("MainCanvas").GetComponent<MainCanvasMono>();
				if (mainCanvas != null)
				{
					AddComponentObject(GetEntity(TransformUsageFlags.None), new MainCanvasComponent{RootCanvas = mainCanvas.MainCanvas, CursorRect = (RectTransform)mainCanvas.CursorMono.transform});
				}
				else
				{
					Debug.Log("Cannot find MainCanvas ");
				}
			}
		}
	}
}