using Code.Map.Building.Systems;
using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class BuildingPlacingInputState :IInputState
    {
        public void OnStateSet()
        {
            Managers.Instance.Cameras.FocusCameraOn(BuildingSystem.CurrentBuilding.transform);
            Managers.Instance.GUI.RequiredResourcesPanel.gameObject.SetActive(true);
            Managers.Instance.GUI.RequiredResourcesPanel.OnPanelOpen();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Systems.Instance.Building.MoveCurrentBuilding(Vector3Int.left);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Systems.Instance.Building.MoveCurrentBuilding(Vector3Int.right);

            if (Input.GetKeyDown(inputManager.Action)) {
                Systems.Instance.Building.BuildBuilding();
            }

            if (Input.GetKeyDown(inputManager.Cancel)) {
                Systems.Instance.Building.CancelBuilding();
                Managers.Instance.Cameras.FocusCameraOn(Managers.Instance.Player.PlayerGO.transform);
                Managers.Instance.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
            Managers.Instance.Cameras.FocusCameraOn(Managers.Instance.Player.PlayerGO.transform);
            Managers.Instance.GUI.RequiredResourcesPanel.OnPanelClose();
        }
    }
}