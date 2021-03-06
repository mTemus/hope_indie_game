using Code.Map.Building.Systems;
using Code.Player;
using Code.Player.Tools;
using Code.System.Area;
using Code.System.Camera;
using Code.System.PlayerInput;
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

      public static Managers Instance => _instance;
   }
}
