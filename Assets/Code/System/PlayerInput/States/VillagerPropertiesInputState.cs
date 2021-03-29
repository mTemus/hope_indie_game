using Code.GUI.UIElements.SelectableElement;
using Code.GUI.Villagers.Selecting;

namespace Code.System.PlayerInput.States
{
    public class VillagerPropertiesInputState : IInputState
    {
        private IInputState currentChildState;
        
        public void OnStateSet()
        {
            currentChildState = new VillagerPropertiesDisplayChildState();
            Managers.Instance.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            currentChildState.HandleState(inputManager);
        }

        public void OnStateChange()
        {
            Managers.Instance.VillagerSelection.SelectedVillager.Profession.enabled = true;
            Managers.Instance.VillagerSelection.DeselectVillager();
            currentChildState = null;
        }

        public void SetToVillagerPropertiesDisplayChildState()
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerPropertiesDisplayChildState();
        }

        public void SetToVillagerProfessionDisplayChildState(VillagerProfessionChangingPanel panel)
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerProfessionDisplayChildInputState(panel);
        }

        public void SetToNewProfessionAcceptChildState(UiAcceptancePanel panel)
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerProfessionSetAcceptanceChildInputState(panel);
        }
    }
}
