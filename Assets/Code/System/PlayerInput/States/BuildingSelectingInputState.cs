using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class BuildingSelectingInputState :IInputState
    {
        public void OnStateSet()
        {
            Managers.Instance.Building.SetBuilding(0);
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.Building.ChangeBuilding(-1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.Building.ChangeBuilding(1);
            
            if (Input.GetKeyDown(inputManager.Action)) 
                inputManager.SetState(InputManager.BuildingPlacingInputState);
        }

        public void OnStateChange()
        {
            
        }
    }
}