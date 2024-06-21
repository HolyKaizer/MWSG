using System;
using Components;
using UnityEngine;

namespace ScriptableConfigs
{
	[CreateAssetMenu(fileName = "ElementalAttackCollectionConfig", menuName = "MWSG/ElementalAttackCollectionConfig", order = 0)]
	public class ElementalAttackCollectionConfig : ScriptableObject
	{
		public ElementalInfos[] Infos;
		
		[Serializable]
		public struct ElementalInfos
		{
			public float Speed;
			public float TravelTime;
			public ElementalType Type;
			public GameObject ProjectilePrefab;
		}
	}
}