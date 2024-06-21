using Unity.Entities;

namespace Components.InfoComponents
{
	public struct ElementalAttackInfosBlobAsset
	{
		public BlobArray<BlobAssetReference<ElementalAttackItemInfo>> ElementsInfo;
	}
}