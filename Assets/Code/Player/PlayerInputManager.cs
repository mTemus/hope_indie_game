using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player
{
    public enum InputState
    {
        WALKING, UI
    }
    
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerToolsManager tools;

        private InputState inputState;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ToggleState();
            }
            
            switch (InputState) {
                case InputState.WALKING:
                    MovePlayerCharacter();
                    UseTool();
                    break;
                
                case InputState.UI:
                    ChangePlayerTool();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MovePlayerCharacter()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
                movement.Move(Vector3.left);
        
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
                movement.Move(Vector3.right);
        }
        
        private void ChangePlayerTool()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
                tools.SelectTool(-1);
        
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
                tools.SelectTool(1);
        }

        private void UseTool()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                tools.UseCurrentTool();
            }
        }

        private void ToggleState()
        {
            inputState = inputState == InputState.WALKING ? InputState.UI : InputState.WALKING;
        }
        
        public InputState InputState
        {
            get => inputState;
            set => inputState = value;
        }
    }
}
