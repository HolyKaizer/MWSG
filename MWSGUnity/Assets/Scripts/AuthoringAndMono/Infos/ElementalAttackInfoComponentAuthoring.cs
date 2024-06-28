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
		public class ElementalAttackInfoComponentBaker : Baker<ElementalAttackInfoComponentAuthoring>
		{
			public override void Bake(ElementalAttackInfoComponentAuthoring authoring)
			{
				var builder = new BlobBuilder(Allocator.Temp);
				
				ref var root = ref builder.ConstructRoot<ElementalAttackInfosBlobAsset>();
				var array = builder.Allocate(ref root.ElementsInfo, authoring.Config.Infos.Length);
				for (var index = 0; index < authoring.Config.Infos.Length; index++)
				{
					var configInfo = authoring.Config.Infos[index];
					var entityPrefab = this.GetEntityPrefab(configInfo.ProjectilePrefab, TransformUsageFlags.None);
					array[index] = new ElementalAttackItemInfo { Type = configInfo.Type, TravelTime = configInfo.Distance / configInfo.Speed, Speed = configInfo.Speed, EntityPrefab = entityPrefab, DamageAmount = configInfo.Damage};
				}

				var blobReference = builder.CreateBlobAssetReference<ElementalAttackInfosBlobAsset>(Allocator.Persistent);
				AddBlobAsset(ref blobReference, out var hash);
				builder.Dispose();
				AddComponent(GetEntity(TransformUsageFlags.None), new ElementalAttackCollectionInfoComponent{ InfosReference = blobReference });
			}
		}
	}
}