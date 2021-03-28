using Code.System;
using Code.System.PlayerInput;

namespace Code.Player.Tools
{
    public class PlayerToolVillagersBook : PlayerTool
    {
        public override void UseTool()
        {
            if (Managers.Instance.VillagerSelection.SelectedVillager != null) return;
            if (!Managers.Instance.VillagerSelection.AreVillagersNearby()) return;
            Managers.Instance.VillagerSelection.SelectVillager();
            Managers.Instance.VillagerSelection.SelectedVillager.Profession.enabled = false;
            Managers.Instance.Input.SetState(InputManager.VillagerPropertiesInputState);
        }
    }
}
