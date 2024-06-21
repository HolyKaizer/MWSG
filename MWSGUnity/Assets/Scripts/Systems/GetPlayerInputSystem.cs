using Components;
using Components.Tags;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	public partial class GetPlayerInputSystem : SystemBase
	{
		private MainMWSG_InputActions _inputActions;
		private Entity _inputEntity;

		protected override void OnCreate()
		{
			RequireForUpdate<BattleCoreComponent>();		
			_inputActions = new MainMWSG_InputActions();
		}

		protected override void OnStartRunning()
		{
			_inputActions.Enable();
			_inputActions.Battle.Enable();
			_inputEntity = SystemAPI.GetSingletonEntity<InputListenerTagComponent>();
			_inputActions.Battle.Attack.performed += OnAttackPerformed;
		}

		private void OnAttackPerformed(InputAction.CallbackContext obj)
		{
			if(!SystemAPI.Exists(_inputEntity)) return;
			
			SystemAPI.SetComponentEnabled<SimpleAttackInputComponent>(_inputEntity, true);
		}

		protected override void OnUpdate()
		{
			var curMoveInput = _inputActions.Battle.Move.ReadValue<Vector2>();
			SystemAPI.SetSingleton(new MoveInput{Value = curMoveInput});
		}
		
		protected override void OnStopRunning()
		{
			_inputActions.Battle.Attack.performed -= OnAttackPerformed;
			_inputActions.Disable();
			_inputActions.Battle.Disable();
			_inputEntity = Entity.Null;
		}
	}
}