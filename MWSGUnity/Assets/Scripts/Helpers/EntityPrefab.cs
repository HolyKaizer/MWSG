using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Scenes;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Can be used in blobs
public struct EntityPrefab
{
	public Unity.Entities.Hash128 guid;
}

public struct ScenePrefabToRegister : IComponentData
{
	public Unity.Entities.Hash128 guid;
	public Entity entity;
}

public struct ScenePrefab : ICleanupComponentData
{
	public Unity.Entities.Hash128 guid;
	public Entity entity;
}

public struct LivingScenePrefab : IComponentData { }

public static class EntityPrefabExtensions
{
#if UNITY_EDITOR
	public static EntityPrefab GetEntityPrefab(this IBaker baker, GameObject prefab, TransformUsageFlags flags)
	{
		Assert.IsTrue(EditorUtility.IsPersistent(prefab), $"The game object '{prefab.name}' is not a prefab.");

		var sceneGUID = baker.GetSceneGUID().Value;
		var prefabGUID = ((Unity.Entities.Hash128)AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(prefab))).Value;

		var guid = new Unity.Entities.Hash128(new uint4(
			sceneGUID.x ^= prefabGUID.x,
			sceneGUID.y ^= prefabGUID.y,
			sceneGUID.z ^= prefabGUID.z,
			sceneGUID.w ^= prefabGUID.w
		));

		// register the prefab in the subscene
		var scenePrefab = baker.CreateAdditionalEntity(TransformUsageFlags.None, entityName: $"ScenePrefab ({prefab.name})");
		baker.AddComponent(scenePrefab, new ScenePrefabToRegister
		{
			guid = guid,
			entity = baker.GetEntity(prefab, flags),
		});

		return new EntityPrefab { guid = guid };
	}
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Entity GetEntity(this in EntityPrefab prefab, in EntityPrefabRegisterSystem.Map map)
	{
		return map.value[prefab.guid];
	}
}

[WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
public partial struct EntityPrefabDeDuplicationBakingSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var uniquePrefabs = new NativeHashSet<Unity.Entities.Hash128>(8, Allocator.Temp);
		var ecb = new EntityCommandBuffer(Allocator.Temp);

		foreach (var (toRegister, entity) in SystemAPI.Query<ScenePrefabToRegister>().WithEntityAccess())
		{
			if (!uniquePrefabs.Add(toRegister.guid))
				ecb.DestroyEntity(entity);
		}

		ecb.Playback(state.EntityManager);
	}
}

[UpdateInGroup(typeof(SceneSystemGroup), OrderLast = true)]
[RequireMatchingQueriesForUpdate]
public partial struct EntityPrefabRegisterSystem : ISystem
{
	private EntityQuery _toRegisterQuery;
	private EntityQuery _toUnregisterQuery;

	public struct Map : IComponentData
	{
		public NativeHashMap<Unity.Entities.Hash128, Entity> value;
	}

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		_toRegisterQuery = SystemAPI.QueryBuilder()
			.WithAll<ScenePrefabToRegister>()
			.Build();

		_toUnregisterQuery = SystemAPI.QueryBuilder()
			.WithAll<ScenePrefab>()
			.WithNone<LivingScenePrefab>()
			.Build();

		state.EntityManager.AddComponentData(state.SystemHandle, new Map
		{
			value = new NativeHashMap<Unity.Entities.Hash128, Entity>(8, Allocator.Persistent),
		});
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
		ref var map = ref SystemAPI.GetComponentRW<Map>(state.SystemHandle).ValueRW.value;
		map.Dispose();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		ref var map = ref SystemAPI.GetComponentRW<Map>(state.SystemHandle).ValueRW.value;

		if (!_toUnregisterQuery.IsEmpty)
		{
			// unregister destroyed scene prefabs
			var entities = _toUnregisterQuery.ToEntityArray(Allocator.Temp);
			var prefabs = _toUnregisterQuery.ToComponentDataArray<ScenePrefab>(Allocator.Temp);

			foreach (var prefab in prefabs)
				map.Remove(prefab.guid);

			state.EntityManager.RemoveComponent<ScenePrefab>(entities);
		}

		if (!_toRegisterQuery.IsEmpty)
		{
			// register new scene prefabs
			var entities = _toRegisterQuery.ToEntityArray(Allocator.Temp);
			var prefabs = _toRegisterQuery.ToComponentDataArray<ScenePrefabToRegister>(Allocator.Temp);

			state.EntityManager.RemoveComponent<ScenePrefabToRegister>(entities);
			state.EntityManager.AddComponent<ScenePrefab>(entities);
			state.EntityManager.AddComponent<LivingScenePrefab>(entities);

			for (var i = 0; i < entities.Length; ++i)
			{
				var prefab = prefabs[i];
				map.Add(prefab.guid, prefab.entity);
				SystemAPI.SetComponent(entities[i], new ScenePrefab
				{
					guid = prefab.guid,
					entity = prefab.entity,
				});
			}
		}
	}
}