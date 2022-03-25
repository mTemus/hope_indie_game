using _Prototype.Code.v001.Characters.Player;
using _Prototype.Code.v001.Characters.Villagers;
using _Prototype.Code.v001.Characters.Villagers.Professions;
using _Prototype.Code.v001.Environment;
using _Prototype.Code.v001.GUI;
using _Prototype.Code.v001.System.Camera;
using _Prototype.Code.v001.System.Initialization;
using _Prototype.Code.v001.System.PlayerTools;
using _Prototype.Code.v001.System.Sound;
using _Prototype.Code.v001.World.Areas;
using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Resources;
using _Prototype.Code.v002.System.GameInput;
using UnityEngine;

namespace _Prototype.Code.v001.System
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
