using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace AuthoringAndMono.MonoScripts
{
	public sealed class CursorMonoComponent : MonoBehaviour
	{
		[SerializeField] private Image CursorIcon;
		[SerializeField] private RectTransform RootTransform;

		public float3 GetCursorWorldPosition()
		{
			return RootTransform.position;
		}
	}
}