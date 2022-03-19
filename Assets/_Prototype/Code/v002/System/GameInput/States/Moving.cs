using _Prototype.Code.v001.System;
using _Prototype.Code.v002.Player;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.System.GameInput.States
{
    /// <summary>
    /// Input state responsible of handling user input for moving player character
    /// </summary>
    public class Moving : IInputState
    {
        private readonly PlayerCharacter _playerCharacter;

        public Moving(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }

        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKey(inputManager.Left) || Input.GetKey(inputManager.LeftAlt)) {
                _playerCharacter.Movement.Move(Vector3.left);
                _playerCharacter.Animations.SetState(PlayerAnimationState.Run);
            }

            if (Input.GetKey(inputManager.Right) || Input.GetKey(inputManager.RightAlt)) {
                _playerCharacter.Movement.Move(Vector3.right);
                _playerCharacter.Animations.SetState(PlayerAnimationState.Run);
            }

            if (!Input.anyKey) 
                _playerCharacter.Animations.SetState(PlayerAnimationState.Idle);

            if (Input.GetKeyDown(inputManager.Action)) 
                Managers.I.Tools.UseCurrentTool();
            
            if (Input.GetKeyDown(inputManager.Tools)) 
                Managers.I.Input.SetState(InputManager.ToolSelecting);
        }

        public void OnStateChange()
        {
            
        }
    }
}