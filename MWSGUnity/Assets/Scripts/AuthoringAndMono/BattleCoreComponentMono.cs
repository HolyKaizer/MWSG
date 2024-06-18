using Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace AuthoringAndMono
{
	public class BattleCoreComponentMono : MonoBehaviour
	{
		public GameObject PlayerCharacterPrefab;
		public uint RandomSeed;
	}

	public class BattleCoreComponentBaker : Baker<BattleCoreComponentMono>
	{
		public override void Bake(BattleCoreComponentMono authoring)
		{
			var battleCoreEntity = GetEntity(authoring, TransformUsageFlags.None);
			AddComponent(battleCoreEntity, new BattleCoreComponent
			{
				PlayerCharacterPrefab = GetEntity(authoring.PlayerCharacterPrefab, TransformUsageFlags.Dynamic)
			});
			
			AddComponent(battleCoreEntity, new BattleCoreRandomComponent
			{
				Value = Random.CreateFromIndex(authoring.RandomSeed)
			});
			
		}
	}
}