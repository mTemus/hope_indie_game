using Code.GUI;
using Code.Map.Building;
using Code.Map.Resources;
using Code.Player;
using Code.Player.Tools;
using Code.System.Areas;
using Code.System.Camera;
using Code.System.GameInput;
using Code.System.Initialization;
using Code.Villagers;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System
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

      [Header("Map")]
      [SerializeField] private AreaManager areas;
      [SerializeField] private BuildingsManager buildings;
      [SerializeField] private ResourcesManager resources;

      [Header("Villagers")]
      [SerializeField] private ProfessionManager professions;
      [SerializeField] private VillagerSelectionManager villagerSelection;
      
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
      public VillagerSelectionManager VillagerSelection => villagerSelection;
      public GUIManager GUI => gui;
      
      private void Awake()
      {
         I = this;
      }
   }
}
