using System;

namespace _Prototype.Code.v002.System.GameInput.States.GUI
{
    /// <summary>
    /// Input state responsible for handling player input while using the player tools GUI
    /// </summary>
    public class ToolSelecting : InputState, IInputState
    {
        public event Action onToolSelected;
        public event Action<int> onMenuValueChanged;
        public event Action<bool> onMenuVisibilityChanged;
        
        public ToolSelecting(global::GameInput gameInput) : base(gameInput)
        {
            gameInput.ToolSelecting.ChangeToolLeft.performed += context => onMenuValueChanged?.Invoke(1);
            gameInput.ToolSelecting.ChangeToolRight.performed += context => onMenuValueChanged?.Invoke(-1);
            gameInput.ToolSelecting.UseTool.performed += context => onToolSelected?.Invoke();
            gameInput.ToolSelecting.UseTool.performed += context => onStateTransition?.Invoke(InputManager.PlayerActions);
        }
        
        public void OnStateSet()
        {
            onMenuVisibilityChanged?.Invoke(true);
            gameInput.ToolSelecting.Enable();
        }

        public void HandleState(InputManager inputManager)
        {
        }

        public void OnStateChange()
        {
            gameInput.ToolSelecting.Disable();
            onMenuVisibilityChanged?.Invoke(false);
        }
    }
}