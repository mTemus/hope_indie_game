using Code.GUI.Villagers.Selecting;
using UnityEngine;

namespace Code.System.GameInput.States
{
    public class VillagerProfessionDisplayChildInputState : IInputState
    {
        private readonly VillagerProfessionChangingPanel professionChangingPanel;

        public VillagerProfessionDisplayChildInputState(VillagerProfessionChangingPanel professionChangingPanel)
        {
            this.professionChangingPanel = professionChangingPanel;
        }

        public void OnStateSet()
        {
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Up) || Input.GetKeyDown(inputManager.UpAlt)) 
                professionChangingPanel.SetPointerOnProfession(-1);
            
            if (Input.GetKeyDown(inputManager.Down) || Input.GetKeyDown(inputManager.DownAlt)) 
                professionChangingPanel.SetPointerOnProfession(1);
            
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                professionChangingPanel.ShowWorkplace(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                professionChangingPanel.ShowWorkplace(1);
            
            if (Input.GetKeyDown(inputManager.Action))
                if (professionChangingPanel.AreThereAnyWorkplaces()) 
                    professionChangingPanel.ShowAcceptancePanel();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                professionChangingPanel.OnPanelClose();
                InputManager.VillagerPropertiesInputState.SetToVillagerPropertiesDisplayChildState();
            }
        }

        public void OnStateChange()
        {
            if (!Managers.I.Cameras.IsCameraOnPlayer())
                Managers.I.Cameras.FocusCameraOnPlayer();
        }
    }
}
