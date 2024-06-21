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
				AddComponent(GetEntity(TransformUsageFlags.None), new TestSpawnProjComponent{TestSpawnPRefab = GetEntity(authoring.TestProj, TransformUsageFlags.None)});
				var builder = new BlobBuilder(Allocator.Temp);
				
				ref var root = ref builder.ConstructRoot<ElementalAttackInfosBlobAsset>();
				var array = builder.Allocate(ref root.ElementsInfo, authoring.Config.Infos.Length);
				for (var index = 0; index < authoring.Config.Infos.Length; index++)
				{
					var configInfo = authoring.Config.Infos[index];
					var entityPrefab = GetEntity(configInfo.ProjectilePrefab, TransformUsageFlags.None);
					array[index] = new ElementalAttackItemInfo { Type = configInfo.Type, TravelTime = configInfo.TravelTime, Speed = configInfo.Speed, EntityPrefab = entityPrefab };
				}

				var blobReference = builder.CreateBlobAssetReference<ElementalAttackInfosBlobAsset>(Allocator.Persistent);
				AddBlobAsset(ref blobReference, out var hash);
				builder.Dispose();
				AddComponent(GetEntity(TransformUsageFlags.None), new ElementalAttackCollectionInfoComponent{ InfosReference = blobReference });
			}
		}
	}
}