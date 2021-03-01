using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class BuildingPlacingInputState :IInputState
    {
        public void OnStateSet()
        {
            //TODO: camera focus on object
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.Building.MoveCurrentBuilding(Vector3Int.left);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.Building.MoveCurrentBuilding(Vector3Int.right);

            if (Input.GetKeyDown(inputManager.Action)) {
                Managers.Instance.Building.BuildBuilding();
            }
        }

        public void OnStateChange()
        {
            //TODO: camera focus on player
        }
    }
}