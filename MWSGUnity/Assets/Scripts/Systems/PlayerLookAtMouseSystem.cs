using Components.Tags;
using Components.UI;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
	[UpdateInGroup(typeof(TransformSystemGroup))]
	[UpdateAfter(typeof(PlayerMoveSystem))]
	public partial struct PlayerLookAtMouseSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<CursorSelectionComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var cursor = SystemAPI.GetSingleton<CursorSelectionComponent>();
			foreach (var transformRef in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<PlayerCharacterTag>())
			{
				var mouseDirection =  cursor.CurrentGroundPoint - transformRef.ValueRO.Position;
				mouseDirection.y = 0;
				transformRef.ValueRW.Rotation = quaternion.LookRotation(mouseDirection, math.up());
			}
		}
	}
}