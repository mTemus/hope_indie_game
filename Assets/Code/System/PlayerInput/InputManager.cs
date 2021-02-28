using System;
using Code.Player;
using Code.System.PlayerInput.States;
using UnityEngine;

namespace Code.System.PlayerInput
{
    public enum InputState
    {
        MOVING,
        TOOL_SELECTING,
        SELECT_BUILDING,
        BUILDING,
    }
    
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;

        [Header("Keycodes")] 
        [SerializeField] private KeyCode left;
        [SerializeField] private KeyCode leftAlt;
        [SerializeField] private KeyCode right;
        [SerializeField] private KeyCode rightAlt;
        [SerializeField] private KeyCode up;
        [SerializeField] private KeyCode upAlt;
        [SerializeField] private KeyCode down;
        [SerializeField] private KeyCode downAlt;
        [SerializeField] private KeyCode action;
        [SerializeField] private KeyCode tools;

        private static MovingInputState _movingInputState;
        private static ToolSelectingInputState _toolSelectingInputState;
        private static BuildingSelectingInputState _buildingSelectingInputState;
        
        private IInputState currentInputState;

        private void Awake()
        {
            _movingInputState = new MovingInputState();
            _toolSelectingInputState = new ToolSelectingInputState();
            _buildingSelectingInputState = new BuildingSelectingInputState();
            
            currentInputState = _movingInputState;
        }

        void Update()
        {
            currentInputState.HandleState(this);
        }

       
        public void SetState(IInputState newInputState)
        {
            currentInputState.OnStateChange();
            currentInputState = newInputState;
            currentInputState.OnStateSet();
        }
        
        public PlayerMovement Movement => movement;

        public KeyCode Left => left;

        public KeyCode LeftAlt => leftAlt;

        public KeyCode Right => right;

        public KeyCode RightAlt => rightAlt;

        public KeyCode Up => up;

        public KeyCode UpAlt => upAlt;

        public KeyCode Down => down;

        public KeyCode DownAlt => downAlt;

        public KeyCode Action => action;

        public KeyCode Tools => tools;

        public static MovingInputState MovingInputState => _movingInputState;

        public static ToolSelectingInputState ToolSelectingInputState => _toolSelectingInputState;

        public static MovingInputState MovingInputState1 => _movingInputState;

        public static ToolSelectingInputState ToolSelectingInputState1 => _toolSelectingInputState;

        public static BuildingSelectingInputState BuildingSelectingInputState => _buildingSelectingInputState;
    }
}
