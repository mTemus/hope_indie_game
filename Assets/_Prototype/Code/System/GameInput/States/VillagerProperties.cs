using _Prototype.Code.GUI.UIElements.SelectableElement;
using _Prototype.Code.GUI.Villager.Selecting;
using _Prototype.Code.System.GameInput.ChildStates;

namespace _Prototype.Code.System.GameInput.States
{
    public class VillagerProperties : IInputState
    {
        private IInputState _currentChildState;
        
        public void OnStateSet()
        {
            _currentChildState = new VillagerPropertiesDisplay();
            Managers.I.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            _currentChildState.HandleState(inputManager);
        }

        public void OnStateChange()
        {
            Managers.I.Selection.SelectedVillager.Profession.enabled = true;
            Managers.I.Selection.DeselectVillager();
            _currentChildState = null;
        }

        public void SetToVillagerPropertiesDisplayChildState()
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerPropertiesDisplay();
        }

        public void SetToVillagerProfessionDisplayChildState(ProfessionChangingPanel panel)
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerProfessionDisplay(panel);
        }

        public void SetToNewProfessionAcceptChildState(UiAcceptancePanel panel)
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerProfessionSetAcceptance(panel);
        }
    }
}
