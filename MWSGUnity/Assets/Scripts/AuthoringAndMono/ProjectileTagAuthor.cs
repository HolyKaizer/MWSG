using Components.Tags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class ProjectileTagAuthor : MonoBehaviour
	{
		public class ProjectileTagBaker : Baker<ProjectileTagAuthor>
		{
			public override void Bake(ProjectileTagAuthor authoring)
			{
				AddComponent(GetEntity(TransformUsageFlags.None), new ProjectileTag());
			}
		}
	}
}