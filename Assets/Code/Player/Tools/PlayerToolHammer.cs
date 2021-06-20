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
            Managers.I.Input.SetState(InputManager.BuildingSelectingInputState);
        }
    }
}
