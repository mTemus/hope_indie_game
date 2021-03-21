using Code.GUI;
using Code.Map.Building;
using Code.Player;
using Code.Player.Tools;
using Code.Resources;
using Code.System.Area;
using Code.System.Camera;
using Code.System.Initialization;
using Code.System.PlayerInput;
using Code.Villagers;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.System
{
   public class Managers : MonoBehaviour
   {
      [SerializeField] private InputManager input;
      [SerializeField] private PlayerToolsManager tools;
      [SerializeField] private PlayerManager player;
      [SerializeField] private CameraManager cameras;
      [SerializeField] private AreaManager areas;
      [SerializeField] private VillagersManager villagers;
      [SerializeField] private BuildingsManager buildings;
      [SerializeField] private TasksManager tasks;
      [SerializeField] private ResourcesManager resources;
      [SerializeField] private ProfessionManager professions;
      [SerializeField] private InitializationManager initialization;
      [SerializeField] private GUIManager gui;
      
      private static Managers _instance;

      private void Awake()
      {
         _instance = this;
      }
      
      public InputManager Input => input;

      public PlayerToolsManager Tools => tools;

      public PlayerManager Player => player;
      
      public CameraManager Cameras => cameras;

      public AreaManager Areas => areas;

      public VillagersManager Villagers => villagers;

      public BuildingsManager Buildings => buildings;

      public TasksManager Tasks => tasks;

      public ResourcesManager Resources => resources;

      public ProfessionManager Professions => professions;

      public InitializationManager Initialization => initialization;

      public GUIManager GUI => gui;

      public static Managers Instance => _instance;
   }
}
