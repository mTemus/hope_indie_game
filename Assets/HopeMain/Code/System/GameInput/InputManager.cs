using HopeMain.Code.AI.Player.Brain;
using HopeMain.Code.System.GameInput.States;
using UnityEngine;

namespace HopeMain.Code.System.GameInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Brain player;

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

        private static Moving moving;
        private static ToolSelecting toolSelecting;
        private static BuildingSelecting buildingSelecting;
        private static BuildingPlacing buildingPlacing;
        private static VillagerProperties villagerProperties;
        
        private IInputState _currentInputState;
        private DeveloperConsole _console;

        private void Awake()
        {
            moving = new Moving();
            toolSelecting = new ToolSelecting();
            buildingSelecting = new BuildingSelecting();
            buildingPlacing = new BuildingPlacing();
            villagerProperties = new VillagerProperties();
            
            _currentInputState = moving;
            _console = new DeveloperConsole();

            Debug.LogWarning(_currentInputState.GetType().Name);
        }

        void Update()
        {
            _console.HandleState(this);

            if (DeveloperTools.Console.DeveloperConsole.I.IsConsoleActive()) return;
            _currentInputState.HandleState(this);
        }
        
        public void SetState(IInputState newInputState)
        {
            _currentInputState.OnStateChange();
            _currentInputState = newInputState;
            _currentInputState.OnStateSet();
            
            Debug.LogWarning("INPUT STATE ----" + _currentInputState.GetType().Name);
        }
        
        public Brain Player => player;

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
    }
}
