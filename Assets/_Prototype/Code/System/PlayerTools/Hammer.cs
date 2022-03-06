using _Prototype.Code.System.GameInput;

namespace _Prototype.Code.System.PlayerTools
{
    public class Hammer : Tool
    {
        public override void UseTool()
        {
            Managers.I.Input.SetState(InputManager.BuildingSelecting);
        }
    }
}
