using _Prototype.Code.v001.AI.Player.Brain;
using UnityEngine;

namespace _Prototype.Code.v001.System.GameInput.States
{
    public class Moving : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKey(inputManager.Left) || Input.GetKey(inputManager.LeftAlt)) {
                inputManager.Player.Motion.Move(Vector3.left);
                inputManager.Player.Animations.SetState(PlayerAnimationState.Run);
            }

            if (Input.GetKey(inputManager.Right) || Input.GetKey(inputManager.RightAlt)) {
                inputManager.Player.Motion.Move(Vector3.right);
                inputManager.Player.Animations.SetState(PlayerAnimationState.Run);
            }

            if (!Input.anyKey) 
                inputManager.Player.Animations.SetState(PlayerAnimationState.Idle);

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