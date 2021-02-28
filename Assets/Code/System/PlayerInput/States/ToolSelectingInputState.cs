
using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class ToolSelectingInputState : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.Tools.SelectTool(-1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.Tools.SelectTool(1);
        }

        public void OnStateChange()
        {
        }
    }
}