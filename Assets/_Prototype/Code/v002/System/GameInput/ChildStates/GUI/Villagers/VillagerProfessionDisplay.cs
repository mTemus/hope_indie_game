using _Prototype.Code.v001.GUI.Villager.Selecting;
using _Prototype.Code.v001.System;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.ChildStates.GUI.Villagers
{
    /// <summary>
    /// Child input state responsible for handling user input when opening 'Villager Profession' panel UI
    /// </summary>
    public class VillagerProfessionDisplay : IInputState
    {
        private readonly ProfessionChangingPanel _professionChangingPanel;

        public VillagerProfessionDisplay(ProfessionChangingPanel professionChangingPanel)
        {
            _professionChangingPanel = professionChangingPanel;
        }

        public void OnStateSet()
        {
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Up) || Input.GetKeyDown(inputManager.UpAlt)) 
                _professionChangingPanel.SetPointerOnProfession(-1);
            
            if (Input.GetKeyDown(inputManager.Down) || Input.GetKeyDown(inputManager.DownAlt)) 
                _professionChangingPanel.SetPointerOnProfession(1);
            
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                _professionChangingPanel.ShowWorkplace(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                _professionChangingPanel.ShowWorkplace(1);
            
            if (Input.GetKeyDown(inputManager.Action))
                if (_professionChangingPanel.AreThereAnyWorkplaces()) 
                    _professionChangingPanel.ShowAcceptancePanel();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                _professionChangingPanel.OnPanelClose();
                InputManager.VillagerProperties.SetToVillagerPropertiesDisplayChildState();
            }
        }

        public void OnStateChange()
        {
            if (!Managers.I.Cameras.IsCameraOnPlayer())
                Managers.I.Cameras.FocusCameraOnPlayer();
        }
    }
}
