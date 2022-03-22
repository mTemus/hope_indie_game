using _Prototype.Code.v001.System;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States.GUI
{
    /// <summary>
    /// Input state responsible for handling player input while using the player tools GUI
    /// </summary>
    public class ToolSelecting : IInputState
    {
        public void OnStateSet()
        {
            Managers.I.GUI.PlayerToolsMenu.Activate();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.I.GUI.PlayerToolsMenu.ChangeCurrentMenuElement(1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.I.GUI.PlayerToolsMenu.ChangeCurrentMenuElement(-1);
            
            if (Input.GetKeyDown(inputManager.Tools)) {
                Managers.I.GUI.PlayerToolsMenu.SelectTool();
                Managers.I.GUI.PlayerToolsMenu.Deactivate();
                Managers.I.Input.SetState(InputManager.PlayerActions);
            }
        }

        public void OnStateChange()
        {
        }
    }
}