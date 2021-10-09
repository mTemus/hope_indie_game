using HopeMain.Code.Characters.Player;
using HopeMain.Code.Characters.Villagers;
using HopeMain.Code.Characters.Villagers.Professions;
using HopeMain.Code.Environment;
using HopeMain.Code.GUI;
using HopeMain.Code.System.Camera;
using HopeMain.Code.System.GameInput;
using HopeMain.Code.System.Initialization;
using HopeMain.Code.System.PlayerTools;
using HopeMain.Code.System.Sound;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.System
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
