using _Prototype.Code.v001.System;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States.GUI
{
    /// <summary>
    /// Input state responsible for handling user input while selecting a building to build
    /// </summary>
    public class BuildingSelecting :IInputState
    {
        public void OnStateSet()
        {
            Managers.I.GUI.BuildingSelectingMenu.gameObject.SetActive(true);
            Managers.I.GUI.BuildingSelectingMenu.OnMenuOpen();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.I.GUI.BuildingSelectingMenu.ChangeBuilding(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.I.GUI.BuildingSelectingMenu.ChangeBuilding(1);
            
            if (Input.GetKeyDown(inputManager.Up) || Input.GetKeyDown(inputManager.UpAlt)) 
                Managers.I.GUI.BuildingSelectingMenu.ChangeBuildingType(-1);
            
            if (Input.GetKeyDown(inputManager.Down) || Input.GetKeyDown(inputManager.DownAlt)) 
                Managers.I.GUI.BuildingSelectingMenu.ChangeBuildingType(1);
            
            if (Input.GetKeyDown(inputManager.Action)) 
                inputManager.SetState(InputManager.BuildingPlacing);
            
            if (Input.GetKeyDown(inputManager.Cancel)) {
                Systems.I.Building.CancelBuilding();
                Managers.I.Input.SetState(InputManager.Moving);
            }
        }

        public void OnStateChange()
        {
            Systems.I.Building.MoveCurrentBuilding(Vector3Int.zero);
            Managers.I.GUI.BuildingSelectingMenu.OnMenuClose();
            Managers.I.GUI.BuildingSelectingMenu.gameObject.SetActive(false);
        }
    }
}