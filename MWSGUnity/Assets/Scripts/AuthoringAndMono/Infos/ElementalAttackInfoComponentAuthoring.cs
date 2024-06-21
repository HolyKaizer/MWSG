using Components;
using Components.InfoComponents;
using ScriptableConfigs;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono.Infos
{
	public class ElementalAttackInfoComponentAuthoring : MonoBehaviour
	{
		public ElementalAttackCollectionConfig Config;
		public GameObject TestProj;
		public class ElementalAttackInfoComponentBaker : Baker<ElementalAttackInfoComponentAuthoring>
		{
			public override void Bake(ElementalAttackInfoComponentAuthoring authoring)
			{
				var blobAsset = CreateBlobAsset(authoring);
				AddComponent(GetEntity(TransformUsageFlags.None), new ElementalAttackCollectionInfoComponent { InfosReference = blobAsset });
				AddComponent(GetEntity(TransformUsageFlags.None), new TestSpawnProjComponent{TestSpawnPRefab = GetEntity(authoring.TestProj, TransformUsageFlags.None)});
			}

			private BlobAssetReference<ElementalAttackInfosBlobAsset> CreateBlobAsset(ElementalAttackInfoComponentAuthoring authoring)
			{
				var builder = new BlobBuilder(Allocator.Temp);
				ref var root = ref builder.ConstructRoot<ElementalAttackInfosBlobAsset>();
				var array = builder.Allocate(ref root.ElementsInfo, authoring.Config.Infos.Length);
				for (var index = 0; index < authoring.Config.Infos.Length; index++)
				{
					var configInfo = authoring.Config.Infos[index];
					var entityPrefab = GetEntity(configInfo.ProjectilePrefab, TransformUsageFlags.None);
					var innerBuilder = new BlobBuilder(Allocator.Temp);
					ref var elementalInfo = ref innerBuilder.ConstructRoot<ElementalAttackItemInfo>();
					innerBuilder.SetPointer(ref elementalInfo.EntityPrefab, ref entityPrefab);
					elementalInfo.Type = configInfo.Type;
					elementalInfo.TravelTime = configInfo.TravelTime;
					elementalInfo.Speed = configInfo.Speed;
					elementalInfo.DamageAmount = configInfo.Damage;
					var innerReference = innerBuilder.CreateBlobAssetReference<ElementalAttackItemInfo>(Allocator.Persistent);
					array[index] = innerReference;
				}

				var blobReference = builder.CreateBlobAssetReference<ElementalAttackInfosBlobAsset>(Allocator.Persistent);
				AddBlobAsset(ref blobReference, out var hash);
				builder.Dispose();
				return blobReference;
			}
		}
	}
}