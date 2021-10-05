using HopeMain.Code.GUI.UIElements.SelectableElement;
using HopeMain.Code.GUI.Villager.Selecting;
using HopeMain.Code.System.GameInput.ChildStates;

namespace HopeMain.Code.System.GameInput.States
{
    public class VillagerPropertiesInputState : IInputState
    {
        private IInputState currentChildState;
        
        public void OnStateSet()
        {
            currentChildState = new VillagerPropertiesDisplayChildState();
            Managers.I.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            currentChildState.HandleState(inputManager);
        }

        public void OnStateChange()
        {
            Managers.I.VillagerSelection.SelectedVillager.Profession.enabled = true;
            Managers.I.VillagerSelection.DeselectVillager();
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
