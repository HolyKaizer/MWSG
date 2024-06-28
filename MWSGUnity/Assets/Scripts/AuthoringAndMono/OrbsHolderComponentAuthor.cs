using Components;
using ScriptableConfigs;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class OrbsHolderComponentAuthor : MonoBehaviour
	{
		public InitialPlayerOrbsConfig Config;
		public class OrbsHolderComponentBaker : Baker<OrbsHolderComponentAuthor>
		{
			public override void Bake(OrbsHolderComponentAuthor authoring)
			{
				var buffer = AddBuffer<OrbBufferElement>(GetEntity(TransformUsageFlags.None));
				foreach (var type in authoring.Config.InitialOrbs)
				{
					buffer.Add(new OrbBufferElement { RuntimeData = new OrbRuntimeData{ Type = type} });
				}
			}
		}
	}
}