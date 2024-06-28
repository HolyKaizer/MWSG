using Components;
using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class InputListenerTagComponentAuthor : MonoBehaviour
	{
		public class InputListenerTagComponentBaker : Baker<InputListenerTagComponentAuthor>
		{
			public override void Bake(InputListenerTagComponentAuthor authoring)
			{
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new InputListenerTagComponent());
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new MoveInput());
				AddComponent(GetEntity(authoring, TransformUsageFlags.None), new SimpleAttackInputComponent());
			}
		}
	}
}