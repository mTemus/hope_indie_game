using _Prototype.Code.Characters.Player;
using _Prototype.Code.Characters.Villagers;
using _Prototype.Code.Characters.Villagers.Professions;
using _Prototype.Code.Environment;
using _Prototype.Code.GUI;
using _Prototype.Code.System.Camera;
using _Prototype.Code.System.GameInput;
using _Prototype.Code.System.Initialization;
using _Prototype.Code.System.PlayerTools;
using _Prototype.Code.System.Sound;
using _Prototype.Code.World.Areas;
using _Prototype.Code.World.Buildings;
using _Prototype.Code.World.Resources;
using UnityEngine;

namespace _Prototype.Code.System
{
   public class Managers : MonoBehaviour
   {
      [Header("Player")]
      [SerializeField] private PlayerToolsManager tools;
      [SerializeField] private PlayerManager player;

      [Header("System")]
      [SerializeField] private InputManager input;
      [SerializeField] private CameraManager cameras;
      [SerializeField] private InitializationManager initialization;
      [SerializeField] private SoundManager sound;

      [Header("Map")]
      [SerializeField] private AreaManager areas;
      [SerializeField] private BuildingsManager buildings;
      [SerializeField] private ResourcesManager resources;

      [Header("Villagers")]
      [SerializeField] private ProfessionManager professions;
      [SerializeField] private SelectionManager selection;

      [Header("Environment")] 
      [SerializeField] private EnvironmentManager environment;
      
      [Header("GUI")]
      [SerializeField] private GUIManager gui;

      public static Managers I { get; private set; }

      public InputManager Input => input;
      public PlayerToolsManager Tools => tools;
      public PlayerManager Player => player;
      public CameraManager Cameras => cameras;
      public AreaManager Areas => areas;
      public BuildingsManager Buildings => buildings;
      public ResourcesManager Resources => resources;
      public ProfessionManager Professions => professions;
      public InitializationManager Initialization => initialization;
      public SelectionManager Selection => selection;
      public GUIManager GUI => gui;
      public SoundManager Sound => sound;
      public EnvironmentManager Environment => environment;

      private void Awake()
      {
         I = this;
      }
   }
}
