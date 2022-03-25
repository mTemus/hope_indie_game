using System;
using _Prototype.Code.v002.Player;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States
{
    /// <summary>
    /// Input state responsible of handling user input for player character actions
    /// </summary>
    public class PlayerActions : InputState, IInputState
    {
        public event Action<Vector3> onMovement;
        public event Action<PlayerAnimationState> onMovementPerformed;
        public event Action<PlayerAnimationState> onMovementCanceled;
        public event Action<IInputState> onToolsSelectingPerformed;
        // public event Action onToolUsePerformed;

        private void HandleMovement()
        {
            Vector2 moveValue = gameInput.PlayerActions.MovePlayer.ReadValue<Vector2>();
            onMovement?.Invoke(new Vector3(moveValue.x, 0, 0));

            if (gameInput.PlayerActions.MovePlayer.WasPerformedThisFrame()) 
                onMovementPerformed?.Invoke(PlayerAnimationState.Run);
        }
        
        public PlayerActions(global::GameInput gameInput) : base(gameInput)
        {
            this.gameInput.PlayerActions.MovePlayer.canceled += context => onMovementCanceled?.Invoke(PlayerAnimationState.Idle);
            // this.gameInput.Moving.UseTool.performed += context => onToolUsePerformed?.Invoke();
            this.gameInput.PlayerActions.OpenTools.performed +=
                context => onToolsSelectingPerformed?.Invoke(InputManager.ToolSelecting);
        }

        public void OnStateSet()
        {
            gameInput.PlayerActions.Enable();
        }
        
        public void HandleState(InputManager inputManager)
        {
           HandleMovement();
        }

        public void OnStateChange()
        {
            gameInput.PlayerActions.Disable();
        }
    }
}