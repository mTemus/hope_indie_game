using HopeMain.Code.System.GameInput;

namespace HopeMain.Code.System.PlayerTools
{
    public class PlayerToolVillagersBook : PlayerTool
    {
        public override void UseTool()
        {
            if (Managers.I.VillagerSelection.SelectedVillager != null) return;
            if (!Managers.I.VillagerSelection.AreVillagersNearby()) return;
            Managers.I.VillagerSelection.SelectVillager();
            Managers.I.VillagerSelection.SelectedVillager.Profession.enabled = false;
            Managers.I.Input.SetState(InputManager.VillagerPropertiesInputState);
        }
    }
}
