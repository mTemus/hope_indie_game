using Code.System;
using Code.System.PlayerInput;

namespace Code.Player.Tools
{
    public class PlayerToolVillagersBook : PlayerTool
    {
        public override void UseTool()
        {
            //TODO: stopVillager to move while getting properties

            Villager villager = Managers.Instance.Player.Player.VillagerToInteract;

            if (villager == null) return;
            villager.Profession.enabled = false;
            Managers.Instance.VillagerSelection.SelectVillager();
            Managers.Instance.Input.SetState(InputManager.VillagerPropertiesInputState);
        }
    }
}
