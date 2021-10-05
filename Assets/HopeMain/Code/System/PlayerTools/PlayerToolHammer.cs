using HopeMain.Code.System.GameInput;

namespace HopeMain.Code.System.PlayerTools
{
    public class PlayerToolHammer : PlayerTool
    {
        public override void UseTool()
        {
            Managers.I.Input.SetState(InputManager.BuildingSelectingInputState);
        }
    }
}
