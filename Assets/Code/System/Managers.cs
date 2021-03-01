using Code.Map.Building.Systems;
using Code.Player;
using Code.Player.Tools;
using Code.System.PlayerInput;
using UnityEngine;

namespace Code.System
{
   public class Managers : MonoBehaviour
   {
      [SerializeField] private InputManager input;
      [SerializeField] private PlayerToolsManager tools;
      [SerializeField] private PlayerManager player;
      [SerializeField] private BuildingManager building;

      private static Managers _instance;

      
      private void Awake()
      {
         _instance = this;
      }
      
      public InputManager Input => input;

      public PlayerToolsManager Tools => tools;

      public PlayerManager Player => player;

      public BuildingManager Building => building;

      public static Managers Instance => _instance;
   }
}
