using Components.ManagedComponents;
using Components.UI;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using Plane = UnityEngine.Plane;

namespace Systems.UI
{
	[UpdateBefore(typeof(TransformSystemGroup))]
	public partial struct CursorUpdatePositionSystem : ISystem
	{
		private Plane _groundPlane;
		public void OnCreate(ref SystemState state)
		{
			_groundPlane = new Plane(Vector3.up, Vector3.zero);
			state.RequireForUpdate<CursorSelectionComponent>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var entityQuery = SystemAPI.QueryBuilder().WithAll<FollowCameraComponent>().Build();
			if (entityQuery.IsEmpty) return;
			foreach (var canvasComponent in SystemAPI.Query<MainCanvasComponent>())
			{
				var camera = SystemAPI.ManagedAPI.GetSingleton<FollowCameraComponent>().UICamera;
				var mousePos = Mouse.current.position.ReadValue();
				var cursorEntity = SystemAPI.GetSingletonEntity<CursorSelectionComponent>();
				var cursorEntityTransform = SystemAPI.GetComponentRW<LocalTransform>(cursorEntity);
				var cursorSelection = SystemAPI.GetComponentRW<CursorSelectionComponent>(cursorEntity);
				var cursorSceneTransform = canvasComponent.CursorRect.transform;
				cursorSceneTransform.position = mousePos;
				cursorEntityTransform.ValueRW.Position = cursorSceneTransform.position;
				var ray = RectTransformUtility.ScreenPointToRay(camera, mousePos);
				
				if (_groundPlane.Raycast(ray, out var enter))
				{
					var point = ray.GetPoint(enter);
					cursorSelection.ValueRW.CurrentGroundPoint = point;
				}
			}
		}

		public void OnDestroy(ref SystemState state)
		{
		}
	}
}