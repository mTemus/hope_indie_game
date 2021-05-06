using Code.Map.Building.Systems;
using UnityEngine;

namespace Code.System.GameInput.States
{
    public class BuildingPlacingInputState :IInputState
    {
        public void OnStateSet()
        {
            Managers.I.Cameras.FocusCameraOn(BuildingSystem.CurrentBuilding.transform);
            Managers.I.GUI.RequiredResourcesPanel.gameObject.SetActive(true);
            Managers.I.GUI.RequiredResourcesPanel.OnPanelOpen();
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
                Managers.I.Cameras.FocusCameraOn(Managers.I.Player.PlayerGO.transform);
                Managers.I.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
            Managers.I.Cameras.FocusCameraOn(Managers.I.Player.PlayerGO.transform);
            Managers.I.GUI.RequiredResourcesPanel.OnPanelClose();
        }
    }
}