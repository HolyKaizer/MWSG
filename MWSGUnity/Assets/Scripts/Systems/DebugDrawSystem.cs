using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class DebugDrawSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities.WithAll<PlayerCharacterTag>().ForEach((in LocalTransform transform) =>
		{
			Debug.DrawLine(transform.Position, transform.Position + new float3(0, 1, 0), Color.red);
		}).WithoutBurst().Run();
	}
}