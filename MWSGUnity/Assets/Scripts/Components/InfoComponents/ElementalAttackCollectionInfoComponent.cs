using Unity.Entities;

namespace Components.InfoComponents
{
	public struct ElementalAttackCollectionInfoComponent : IComponentData
	{
		public BlobAssetReference<ElementalAttackInfosBlobAsset> InfosReference;
	}
} 