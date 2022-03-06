using _Prototype.Code.GUI.UIElements.SelectableElement;
using UnityEngine;

namespace _Prototype.Code.System.GameInput.ChildStates
{
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
