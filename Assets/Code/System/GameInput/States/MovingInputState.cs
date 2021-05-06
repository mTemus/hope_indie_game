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
            if (Input.GetKey(inputManager.Left) || Input.GetKey(inputManager.LeftAlt)) 
                inputManager.Movement.Move(Vector3.left);
        
            if (Input.GetKey(inputManager.Right) || Input.GetKey(inputManager.RightAlt)) 
                inputManager.Movement.Move(Vector3.right);

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