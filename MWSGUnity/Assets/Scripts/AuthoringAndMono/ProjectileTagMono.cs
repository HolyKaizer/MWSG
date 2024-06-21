using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class ProjectileTagMono : MonoBehaviour
	{
		public class ProjectileTagBaker : Baker<ProjectileTagMono>
		{
			public override void Bake(ProjectileTagMono authoring)
			{
				AddComponent(GetEntity(TransformUsageFlags.None), new ProjectileTag());
			}
		}
	}
}