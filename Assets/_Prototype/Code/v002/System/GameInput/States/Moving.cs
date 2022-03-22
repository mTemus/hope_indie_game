using _Prototype.Code.v001.System;
using _Prototype.Code.v002.Player;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States
{
    /// <summary>
    /// Input state responsible of handling user input for moving player character
    /// </summary>
    public class Moving : InputState, IInputState
    {
        private readonly PlayerCharacter _playerCharacter;
        
        public Moving(global::GameInput gameInput, PlayerCharacter playerCharacter) : base(gameInput)
        {
            _playerCharacter = playerCharacter;
            this.gameInput.Moving.MovePlayer.canceled += context => _playerCharacter.Animations.SetState(PlayerAnimationState.Idle);
            this.gameInput.Moving.UseTool.performed += context => Managers.I.Tools.UseCurrentTool();
            this.gameInput.Moving.OpenTools.performed += context => Managers.I.Input.SetState(InputManager.ToolSelecting);
        }

        public void OnStateSet()
        {
            gameInput.Moving.Enable();
        }
        
        public void HandleState(InputManager inputManager)
        {
           Vector2 moveValue = gameInput.Moving.MovePlayer.ReadValue<Vector2>();
            _playerCharacter.Movement.Move(new Vector3(moveValue.x, 0, 0));

            if (gameInput.Moving.MovePlayer.WasPerformedThisFrame()) 
                _playerCharacter.Animations.SetState(PlayerAnimationState.Run);
        }

        public void OnStateChange()
        {
            gameInput.Moving.Disable();
        }
    }
}