using Components;
using UnityEngine;

namespace ScriptableConfigs
{
	[CreateAssetMenu(fileName = "InitialPlayerOrbsConfig", menuName = "MWSG/InitialPlayerOrbsConfig", order = 0)]
	public class InitialPlayerOrbsConfig : ScriptableObject
	{
		public ElementalType[] InitialOrbs;
	}
}