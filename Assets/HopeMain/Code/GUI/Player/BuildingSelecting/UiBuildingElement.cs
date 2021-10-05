using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Buildings.Workplace;
using UnityEngine;

namespace HopeMain.Code.GUI.Player.BuildingSelecting
{
   public class UiBuildingElement : MonoBehaviour
   {
      [SerializeField] private BuildingData data;
      [SerializeField] private WorkplaceProperties properties;
      [SerializeField] private Sprite sprite;
      [SerializeField] [TextArea] private string description;
      
      public string Description => description;

      public BuildingData Data => data;

      public Sprite Sprite => sprite;

      public WorkplaceProperties Properties => properties;
   }
}
