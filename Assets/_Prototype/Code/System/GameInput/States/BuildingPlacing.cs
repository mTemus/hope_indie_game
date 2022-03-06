using _Prototype.Code.World.Buildings.Systems;
using UnityEngine;

namespace _Prototype.Code.System.GameInput.States
{
    public class BuildingPlacing :IInputState
    {
        public void OnStateSet()
        {
            Systems.I.Building.MoveCurrentBuilding(Vector3Int.zero);
            Managers.I.Cameras.FocusCameraOn(BuildingSystem.CurrentBuilding.transform);
            Managers.I.GUI.RequiredResourcesPanel.gameObject.SetActive(true);
            Managers.I.GUI.RequiredResourcesPanel.OnPanelOpen();
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Systems.I.Building.MoveCurrentBuilding(Vector3Int.left);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Systems.I.Building.MoveCurrentBuilding(Vector3Int.right);

            if (Input.GetKeyDown(inputManager.Action)) {
                Systems.I.Building.BuildBuilding();
            }

            if (Input.GetKeyDown(inputManager.Cancel)) {
                Systems.I.Building.CancelBuilding();
                Managers.I.Cameras.FocusCameraOn(Managers.I.Player.PlayerGO.transform);
                Managers.I.Input.SetState(InputManager.Moving);
            }
        }

        public void OnStateChange()
        {
            Managers.I.Cameras.FocusCameraOn(Managers.I.Player.PlayerGO.transform);
            Managers.I.GUI.RequiredResourcesPanel.OnPanelClose();
        }
    }
}