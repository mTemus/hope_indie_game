using UnityEngine;

namespace Code.System.PlayerInput.States
{
    public class VillagerPropertiesInputState : IInputState
    {
        public void OnStateSet()
        {
            Managers.Instance.GUI.VillagerPropertiesPanel.gameObject.SetActive(true);
        }

        public void HandleState(InputManager inputManager)
        {
            // A - D -> change selection pointer
            // E -> use selection
            // Exit -> reset camera -> end state
        
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.Instance.GUI.VillagerPropertiesPanel.MovePointer(-1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.Instance.GUI.VillagerPropertiesPanel.MovePointer(1);
        
            if (Input.GetKeyDown(inputManager.Action)) 
                Managers.Instance.GUI.VillagerPropertiesPanel.UseSelectedElement();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                if (!Managers.Instance.Cameras.IsCameraOnPlayer()) {
                    Managers.Instance.Cameras.FocusCameraOnPlayer();
                    return;
                }
            
                Managers.Instance.GUI.VillagerPropertiesPanel.gameObject.SetActive(false);
                Managers.Instance.Input.SetState(InputManager.MovingInputState);
            }
        }

        public void OnStateChange()
        {
            Managers.Instance.Player.Player.VillagerToInteract.Profession.enabled = true;
        }
    }
}
