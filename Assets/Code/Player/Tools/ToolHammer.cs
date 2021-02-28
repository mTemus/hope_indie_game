using Code.System;
using Code.System.PlayerInput;

namespace Code.Player.Tools
{
    public class ToolHammer : Tool
    {
        public override void UseTool()
        {
            Managers.Instance.Input.SetState(InputManager.BuildingSelectingInputState);
        }
    }
}
