using Code.System;
using Code.System.Area;
using Code.System.PlayerInput;
using UnityEngine;

namespace Code.Player.Tools
{
    public class ToolHammer : Tool
    {
        public override void UseTool()
        {
            if (Managers.Instance.Areas.GetPlayerArea().Type != AreaType.VILLAGE) {
                Debug.LogError("Can't build outside village!");
                return;
            }
            
            Managers.Instance.Input.SetState(InputManager.BuildingSelectingInputState);
        }
    }
}
