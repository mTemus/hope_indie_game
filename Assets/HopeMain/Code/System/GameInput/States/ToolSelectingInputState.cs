﻿
using UnityEngine;

namespace HopeMain.Code.System.GameInput.States
{
    public class ToolSelectingInputState : IInputState
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
                Managers.I.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
        }
    }
}