using _Prototype.Code.System.GameInput;

namespace _Prototype.Code.System.PlayerTools
{
    public class VillagersBook : Tool
    {
        public override void UseTool()
        {
            if (Managers.I.Selection.SelectedVillager != null) return;
            if (!Managers.I.Selection.AreVillagersNearby()) return;
            Managers.I.Selection.SelectVillager();
            Managers.I.Selection.SelectedVillager.Profession.enabled = false;
            Managers.I.Input.SetState(InputManager.VillagerProperties);
        }
    }
}