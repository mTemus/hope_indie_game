using Code.GUI.UIElements.SelectableElement;
using Code.GUI.Villagers.Selecting;

namespace Code.System.PlayerInput.States
{
    public class VillagerPropertiesInputState : IInputState
    {
        private IInputState currentChildState;
        
        public void OnStateSet()
        {
            SetToVillagerPropertiesDisplayChildState();
            Managers.Instance.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            currentChildState.HandleState(inputManager);
        }

        public void OnStateChange()
        {
            Managers.Instance.Player.Player.VillagerToInteract.Profession.enabled = true;
            currentChildState = null;
        }

        public void SetToVillagerPropertiesDisplayChildState()
        {
            currentChildState = new VillagerPropertiesDisplayChildState();
        }

        public void SetToVillagerProfessionDisplayChildState(VillagerProfessionChangingPanel panel)
        {
            currentChildState = new VillagerProfessionDisplayChildInputState(panel);
        }

        public void SetToNewProfessionAcceptChildState(UiAcceptancePanel panel)
        {
            currentChildState = new VillagerProfessionSetAcceptanceChildInputState(panel);
        }
    }
}
