using HopeMain.Code.System.GameInput;

namespace HopeMain.Code.System.PlayerTools
{
    public class Hammer : Tool
    {
        public override void UseTool()
        {
            Managers.I.Input.SetState(InputManager.BuildingSelecting);
        }
    }
}
