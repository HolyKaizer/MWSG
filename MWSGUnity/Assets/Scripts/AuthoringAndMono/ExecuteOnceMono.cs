using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public sealed class ExecuteOnceMono : MonoBehaviour
	{
	}

	public sealed class ExecuteOnceBaker : Baker<ExecuteOnceMono>
	{
		public override void Bake(ExecuteOnceMono authoring)
		{
			AddComponent(GetEntity(authoring, TransformUsageFlags.None), new ExecuteOnceTag());
		}
	}
}