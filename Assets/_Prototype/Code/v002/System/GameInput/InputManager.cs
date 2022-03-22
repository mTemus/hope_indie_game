using _Prototype.Code.v002.Player;
using _Prototype.Code.v002.System.GameInput.States;
using _Prototype.Code.v002.System.GameInput.States.GUI;
using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput
{
    /// <summary>
    /// Manager responsible for handling user input from keyboard/gamepad etc, and process it into different input states
    /// </summary>
    public class InputManager : MonoBehaviour, IManualUpdate
    {
        [Header("Player Character")]
        [SerializeField] private PlayerCharacter player;

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

        private global::GameInput _gameInput;
        
        private static Moving moving;
        private static ToolSelecting toolSelecting;
        private static BuildingSelecting buildingSelecting;
        private static BuildingPlacing buildingPlacing;
        private static VillagerProperties villagerProperties;
        
        private IInputState _currentInputState;
        // private DeveloperConsole _console;

        public PlayerCharacter Player => player;
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

        public static Moving Moving => moving;
        public static ToolSelecting ToolSelecting => toolSelecting;
        public static BuildingSelecting BuildingSelecting => buildingSelecting;
        public static BuildingPlacing BuildingPlacing => buildingPlacing;
        public static VillagerProperties VillagerProperties => villagerProperties;
        
        private void Awake()
        {
            _gameInput = new global::GameInput();
            
            moving = new Moving(_gameInput, player);
            toolSelecting = new ToolSelecting();
            buildingSelecting = new BuildingSelecting();
            buildingPlacing = new BuildingPlacing();
            villagerProperties = new VillagerProperties();
            
            _currentInputState = moving;
            _currentInputState.OnStateSet();
            // _console = new DeveloperConsole();

            Debug.LogWarning(_currentInputState.GetType().Name);
        }
        
        /// <summary>
        /// Method that should be called if new input state should be set
        /// </summary>
        /// <param name="newInputState">New state that should be set</param>
        public void SetState(IInputState newInputState)
        {
            _currentInputState.OnStateChange();
            _currentInputState = newInputState;
            _currentInputState.OnStateSet();
            
            Debug.LogWarning("INPUT STATE ----" + _currentInputState.GetType().Name);
        }

        public void ManualUpdate(float timeSpeed)
        {
            // _console.HandleState(this);

            // if (v001.DeveloperTools.Console.DeveloperConsole.I.IsConsoleActive()) return;
            _currentInputState.HandleState(this);
        }
    }
}
