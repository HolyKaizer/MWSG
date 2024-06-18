using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
	[BurstCompile]
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial struct SpawnPlayerCharacterSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BattleCoreComponent>();
		}
		
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Enabled = false;
			var battleCore = SystemAPI.GetSingleton<BattleCoreComponent>();
			var ecb = new EntityCommandBuffer(Allocator.Temp);
			ecb.Instantiate(battleCore.PlayerCharacterPrefab);
		}
	} 
}