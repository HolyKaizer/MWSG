using Components;
using ScriptableConfigs;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
	public class OrbsHolderComponentMono : MonoBehaviour
	{
		public InitialPlayerOrbsConfig Config;
		public class OrbsHolderComponentBaker : Baker<OrbsHolderComponentMono>
		{
			public override void Bake(OrbsHolderComponentMono authoring)
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