using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class BuildingSelectingInputState :IInputState
    {
        public void OnStateSet()
        {
            Managers.Instance.GUI.BuildingSelectingMenu.gameObject.SetActive(true);
            Managers.Instance.GUI.BuildingSelectingMenu.OnMenuOpen();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.GUI.BuildingSelectingMenu.ChangeBuilding(-1);
            
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.GUI.BuildingSelectingMenu.ChangeBuilding(1);
            
            if (Input.GetKeyDown(inputManager.Up) || Input.GetKeyDown(inputManager.UpAlt)) 
                Managers.Instance.GUI.BuildingSelectingMenu.ChangeBuildingType(1);
            
            if (Input.GetKeyDown(inputManager.Down) || Input.GetKeyDown(inputManager.DownAlt)) 
                Managers.Instance.GUI.BuildingSelectingMenu.ChangeBuildingType(-1);
            
            if (Input.GetKeyDown(inputManager.Action)) 
                inputManager.SetState(InputManager.BuildingPlacingInputState);
            
            if (Input.GetKeyDown(inputManager.Cancel)) {
                Systems.Instance.Building.CancelBuilding();
                Managers.Instance.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
            Managers.Instance.GUI.BuildingSelectingMenu.OnMenuClose();
            Managers.Instance.GUI.BuildingSelectingMenu.gameObject.SetActive(false);
        }
    }
}