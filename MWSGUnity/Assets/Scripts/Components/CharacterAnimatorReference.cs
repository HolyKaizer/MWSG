using Unity.Entities;
using UnityEngine;

namespace Components
{
	public class CharacterAnimatorReference : ICleanupComponentData
	{
		public Animator Value;
	}
}