using Code.System;
using Code.System.Areas;
using Code.System.GameInput;
using UnityEngine;

namespace Code.Player.Tools
{
    public class PlayerToolHammer : PlayerTool
    {
        public override void UseTool()
        {
            if (Managers.I.Areas.GetPlayerArea().Type != AreaType.VILLAGE) {
                Debug.LogError("Can't build outside village!");
                return;
            }
            
            Managers.I.Input.SetState(InputManager.BuildingSelectingInputState);
        }
    }
}
