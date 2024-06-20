using Components;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	public partial class GetPlayerInputSystem : SystemBase
	{
		private MainMWSG_InputActions _inputActions;
		private Entity _playerEntity;

		protected override void OnCreate()
		{
			RequireForUpdate<PlayerCharacterTagComponent>();		
			_inputActions = new MainMWSG_InputActions();
		}

		protected override void OnStartRunning()
		{
			_inputActions.Enable();
			_inputActions.Battle.Enable();
			_playerEntity = SystemAPI.GetSingletonEntity<PlayerCharacterTagComponent>();
		}

		protected override void OnUpdate()
		{
			var curMoveInput = _inputActions.Battle.Move.ReadValue<Vector2>();
			SystemAPI.SetSingleton(new MoveInput{Value = curMoveInput});
		}
		
		protected override void OnStopRunning()
		{
			_inputActions.Disable();
			_inputActions.Battle.Disable();
			_playerEntity = Entity.Null;
		}
	}
}