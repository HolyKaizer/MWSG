using Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace AuthoringAndMono
{
	public sealed class BattleCoreComponentMono : MonoBehaviour
	{
		public uint RandomSeed;
	}

	public sealed class BattleCoreComponentBaker : Baker<BattleCoreComponentMono>
	{
		public override void Bake(BattleCoreComponentMono authoring)
		{
			var battleCoreEntity = GetEntity(authoring, TransformUsageFlags.None);
			AddComponent(battleCoreEntity, new BattleCoreComponent());
			AddComponent(battleCoreEntity, new BattleCoreRandomComponent
			{
				Value = Random.CreateFromIndex(authoring.RandomSeed)
			});
		}
	}
}