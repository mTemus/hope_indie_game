using _Prototype.Code.v001.System;
using _Prototype.Code.v002.Player;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States
{
    /// <summary>
    /// Input state responsible of handling user input for moving player character
    /// </summary>
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