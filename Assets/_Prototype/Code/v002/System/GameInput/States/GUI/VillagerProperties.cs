using _Prototype.Code.v001.GUI.UIElements.SelectableElement;
using _Prototype.Code.v001.GUI.Villager.Selecting;
using _Prototype.Code.v001.System;
using _Prototype.Code.v002.System.GameInput.ChildStates.GUI.Villagers;

namespace _Prototype.Code.v002.System.GameInput.States.GUI
{
    /// <summary>
    /// Input state responsible for handling user input while using the 'Villager Properties' GUI
    /// </summary>
    public class VillagerProperties : IInputState
    {
        private IInputState _currentChildState;

        private VillagerPropertiesDisplay _villagerPropertiesDisplay;
        private VillagerProfessionDisplay _villagerProfessionDisplay;
        private VillagerProfessionSetAcceptance _villagerProfessionSetAcceptance;

        // public VillagerProperties()
        // {
        //     _villagerPropertiesDisplay = new VillagerPropertiesDisplay();
        //     _villagerProfessionDisplay = new VillagerProfessionDisplay(); 
        //     _villagerProfessionSetAcceptance = new VillagerProfessionSetAcceptance();
        // }

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

        // TODO: these three should be reworked for sure
            // TODO: create sub-states in state constructor
        
        /// <summary>
        /// Set current villager properties panel to main panel
        /// </summary>
        public void SetToVillagerPropertiesDisplayChildState()
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerPropertiesDisplay();
            _currentChildState.OnStateSet();
        }

        /// <summary>
        /// Set current villager properties panel to villager profession changing panel
        /// </summary>
        /// <param name="panel">ProfessionChangingPanel object</param>
        public void SetToVillagerProfessionDisplayChildState(ProfessionChangingPanel panel)
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerProfessionDisplay(panel);
            _currentChildState.OnStateSet();
        }

        /// <summary>
        /// Set current villager properties panel to villager profession acceptance panel
        /// </summary>
        /// <param name="panel">UiAcceptancePanel object</param>
        public void SetToNewProfessionAcceptChildState(UiAcceptancePanel panel)
        {
            _currentChildState.OnStateChange();
            _currentChildState = new VillagerProfessionSetAcceptance(panel);
            _currentChildState.OnStateSet();
        }
    }
}
