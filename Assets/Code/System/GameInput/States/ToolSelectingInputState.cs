
using UnityEngine;

namespace Code.System.GameInput.States
{
    public class ToolSelectingInputState : IInputState
    {
        public void OnStateSet()
        {
            Managers.Instance.GUI.PlayerToolsMenu.Activate();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.GUI.PlayerToolsMenu.ChangeCurrentMenuElement(1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.GUI.PlayerToolsMenu.ChangeCurrentMenuElement(-1);
            
            if (Input.GetKeyDown(inputManager.Tools)) {
                Managers.Instance.GUI.PlayerToolsMenu.SelectTool();
                Managers.Instance.GUI.PlayerToolsMenu.Deactivate();
                Managers.Instance.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
        }
    }
}