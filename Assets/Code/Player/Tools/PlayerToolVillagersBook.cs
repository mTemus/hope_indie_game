using Code.System;
using Code.System.GameInput;

namespace Code.Player.Tools
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
