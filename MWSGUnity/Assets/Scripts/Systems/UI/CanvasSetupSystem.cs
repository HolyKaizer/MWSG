using Components.ManagedComponents;
using Components.Tags;
using Components.UI;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems.UI
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateBefore(typeof(ExecuteOnceTagSystem))]
    public partial struct CanvasSetupSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            Cursor.visible = false;
        }

        public void OnUpdate(ref SystemState state)
        { 
            var query = SystemAPI.QueryBuilder().WithAll<ExecuteOnceTag>().WithAll<MainCanvasComponent>().Build();
            if(query.IsEmpty) return;
            
            var canvasComponent = query.GetSingleton<MainCanvasComponent>();
            var cursorTrans = canvasComponent.CursorRect.transform;
            var cursorEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(cursorEntity, new LocalTransform
            {
                Position =cursorTrans.position,
                Rotation =  cursorTrans.rotation,
                Scale = 1
            });
            state.EntityManager.AddComponentData(cursorEntity, new CursorSelectionComponent{CursorToDrawType = CursorToDraw.Default});
        }
    }
}
