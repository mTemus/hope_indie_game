using Code.Map.Building.Systems;
using Code.Player;
using UnityEngine;

namespace Code.System
{
   public class Managers : MonoBehaviour
   {
      [SerializeField] private InputManager input;
      [SerializeField] private PlayerToolsManager tools;
      [SerializeField] private PlayerManager player;
      [SerializeField] private BuildingManager building;


      public InputManager Input => input;

      public PlayerToolsManager Tools => tools;

      public PlayerManager Player => player;

      public BuildingManager Building => building;

      public Managers Instance => this;
   }
}
