using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class ExecuteOnceAuthor : MonoBehaviour
	{
	}

	public sealed class ExecuteOnceBaker : Baker<ExecuteOnceAuthor>
	{
		public override void Bake(ExecuteOnceAuthor authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new ExecuteOnceTag());
		}
	}
}