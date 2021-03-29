using Code.GUI.UIElements.SelectableElement;
using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class VillagerProfessionSetAcceptanceChildInputState : IInputState
    {
        private readonly UiAcceptancePanel acceptancePanel;

        public VillagerProfessionSetAcceptanceChildInputState(UiAcceptancePanel acceptancePanel)
        {
            this.acceptancePanel = acceptancePanel;
        }

        public void OnStateSet()
        {
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                acceptancePanel.MovePointer(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                acceptancePanel.MovePointer(1);
            
            if (Input.GetKeyDown(inputManager.Action)) 
                acceptancePanel.UseSelectedElement();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                acceptancePanel.gameObject.SetActive(false);
                InputManager.VillagerPropertiesInputState.SetToVillagerProfessionDisplayChildState(Managers.Instance.GUI.VillagerProfessionChangingPanel);
            }
            
        }

        public void OnStateChange()
        {
        }
    }
}
