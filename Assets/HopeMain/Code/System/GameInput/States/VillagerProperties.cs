using HopeMain.Code.GUI.UIElements.SelectableElement;
using HopeMain.Code.GUI.Villager.Selecting;
using HopeMain.Code.System.GameInput.ChildStates;

namespace HopeMain.Code.System.GameInput.States
{
    public class VillagerProperties : IInputState
    {
        private IInputState currentChildState;
        
        public void OnStateSet()
        {
            currentChildState = new VillagerPropertiesDisplay();
            Managers.I.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            currentChildState.HandleState(inputManager);
        }

        public void OnStateChange()
        {
            Managers.I.Selection.SelectedVillager.Profession.enabled = true;
            Managers.I.Selection.DeselectVillager();
            currentChildState = null;
        }

        public void SetToVillagerPropertiesDisplayChildState()
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerPropertiesDisplay();
        }

        public void SetToVillagerProfessionDisplayChildState(ProfessionChangingPanel panel)
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerProfessionDisplay(panel);
        }

        public void SetToNewProfessionAcceptChildState(UiAcceptancePanel panel)
        {
            currentChildState.OnStateChange();
            currentChildState = new VillagerProfessionSetAcceptance(panel);
        }
    }
}
