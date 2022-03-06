using UnityEngine;

namespace _Prototype.Code.System.GameInput.ChildStates
{
    public class VillagerPropertiesDisplay : IInputState
    {
        public void OnStateSet()
        {
        }

        public void HandleState(InputManager inputManager)
        {
            // A - D -> change selection pointer
            // E -> use selection
            // Exit -> reset camera -> end state
            
            if (Input.GetKeyDown(inputManager.Left) || Input.GetKeyDown(inputManager.LeftAlt)) 
                Managers.I.GUI.VillagerPropertiesPanel.MovePointer(-1);
        
            if (Input.GetKeyDown(inputManager.Right) || Input.GetKeyDown(inputManager.RightAlt)) 
                Managers.I.GUI.VillagerPropertiesPanel.MovePointer(1);
        
            if (Input.GetKeyDown(inputManager.Action)) 
                Managers.I.GUI.VillagerPropertiesPanel.UseSelectedElement();

            if (Input.GetKeyDown(inputManager.Cancel)) {
                Managers.I.GUI.VillagerPropertiesPanel.gameObject.SetActive(false);
                Managers.I.Cameras.FocusCameraOnPlayer();
                Managers.I.Input.SetState(InputManager.Moving);
            }
        }

        public void OnStateChange()
        {
        }
    }
}
