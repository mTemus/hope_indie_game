using HopeMain.Code.AI.Player.Brain;
using HopeMain.Code.DeveloperTools.Console;
using HopeMain.Code.System.GameInput.States;
using UnityEngine;

namespace HopeMain.Code.System.GameInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Player_Brain player;

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
        [SerializeField] private KeyCode cancel;
        [SerializeField] private KeyCode console;
        [SerializeField] private KeyCode accept;

        private static MovingInputState _movingInputState;
        private static ToolSelectingInputState _toolSelectingInputState;
        private static BuildingSelectingInputState _buildingSelectingInputState;
        private static BuildingPlacingInputState _buildingPlacingInputState;
        private static VillagerPropertiesInputState _villagerPropertiesInputState;
        
        private IInputState currentInputState;
        private DeveloperConsoleInputState consoleInputState;

        private void Awake()
        {
            _movingInputState = new MovingInputState();
            _toolSelectingInputState = new ToolSelectingInputState();
            _buildingSelectingInputState = new BuildingSelectingInputState();
            _buildingPlacingInputState = new BuildingPlacingInputState();
            _villagerPropertiesInputState = new VillagerPropertiesInputState();
            
            currentInputState = _movingInputState;
            consoleInputState = new DeveloperConsoleInputState();

            Debug.LogWarning(currentInputState.GetType().Name);
        }

        void Update()
        {
            consoleInputState.HandleState(this);

            if (DeveloperConsole.I.IsConsoleActive()) return;
            currentInputState.HandleState(this);
        }
        
        public void SetState(IInputState newInputState)
        {
            currentInputState.OnStateChange();
            currentInputState = newInputState;
            currentInputState.OnStateSet();
            
            Debug.LogWarning("INPUT STATE ----" + currentInputState.GetType().Name);
        }
        
        public Player_Brain Player => player;

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
        public KeyCode Cancel => cancel;
        public KeyCode Console => console;
        public KeyCode Accept => accept;

        public static MovingInputState MovingInputState => _movingInputState;

        public static ToolSelectingInputState ToolSelectingInputState => _toolSelectingInputState;
        
        public static BuildingSelectingInputState BuildingSelectingInputState => _buildingSelectingInputState;

        public static BuildingPlacingInputState BuildingPlacingInputState => _buildingPlacingInputState;

        public static VillagerPropertiesInputState VillagerPropertiesInputState => _villagerPropertiesInputState;
    }
}
