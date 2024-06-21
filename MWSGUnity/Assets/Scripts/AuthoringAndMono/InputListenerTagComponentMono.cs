using Components;
using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class InputListenerTagComponentMono : MonoBehaviour
	{
		public class InputListenerTagComponentBaker : Baker<InputListenerTagComponentMono>
		{
			public override void Bake(InputListenerTagComponentMono authoring)
			{
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new InputListenerTagComponent());
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new MoveInput());
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new SimpleAttackInputComponent());
			}
		}
	}
}