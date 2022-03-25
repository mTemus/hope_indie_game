using _Prototype.Code.v002.System.GameInput;

namespace _Prototype.Code.v001.System.PlayerTools
{
    public class Hammer : Tool
    {
        public override void UseTool()
        {
            Managers.I.Input.SetState(InputManager.BuildingSelecting);
        }
    }
}
