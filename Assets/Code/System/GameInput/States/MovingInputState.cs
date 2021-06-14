using Code.Player;
using UnityEngine;

namespace Code.System.GameInput.States
{
    public class MovingInputState : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKey(inputManager.Left) || Input.GetKey(inputManager.LeftAlt)) {
                inputManager.Player.Movement.Move(Vector3.left);
                inputManager.Player.Animations.SetState(PlayerAnimationState.Run);
            }

            if (Input.GetKey(inputManager.Right) || Input.GetKey(inputManager.RightAlt)) {
                inputManager.Player.Movement.Move(Vector3.right);
                inputManager.Player.Animations.SetState(PlayerAnimationState.Run);
            }

            if (!Input.anyKey) 
                inputManager.Player.Animations.SetState(PlayerAnimationState.Idle);

            if (Input.GetKeyDown(inputManager.Action)) 
                Managers.I.Tools.UseCurrentTool();
            
            if (Input.GetKeyDown(inputManager.Tools)) 
                Managers.I.Input.SetState(InputManager.ToolSelectingInputState);
        }

        public void OnStateChange()
        {
            
        }
    }
}