using _Prototype.Code.v001.GUI.UIElements.SelectableElement;
using _Prototype.Code.v001.System;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.ChildStates.GUI.Villagers
{
    /// <summary>
    /// Child input state responsible for handling user input when opening 'Villager Profession Acceptance' panel UI
    /// </summary>
    public class VillagerProfessionSetAcceptance : IInputState
    {
        private readonly UiAcceptancePanel _acceptancePanel;
        
        public VillagerProfessionSetAcceptance(UiAcceptancePanel acceptancePanel)
        {
            _acceptancePanel = acceptancePanel;
        }

        public void OnStateSet()
        {
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                _acceptancePanel.MovePointer(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                _acceptancePanel.MovePointer(1);
            
            if (Input.GetKeyDown(inputManager.Action)) 
                _acceptancePanel.UseSelectedElement();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                _acceptancePanel.gameObject.SetActive(false);
                InputManager.VillagerProperties.SetToVillagerProfessionDisplayChildState(Managers.I.GUI.ProfessionChangingPanel);
            }
            
        }

        public void OnStateChange()
        {
        }
    }
}
