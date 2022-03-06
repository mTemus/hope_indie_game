using _Prototype.Code.World.Buildings;
using _Prototype.Code.World.Buildings.Workplaces;
using UnityEngine;

namespace _Prototype.Code.GUI.Player.BuildingSelecting
{
   /// <summary>
   /// 
   /// </summary>
   public class UiBuildingElement : MonoBehaviour
   {
      [SerializeField] private Data data;
      [SerializeField] private Properties properties;
      [SerializeField] private Sprite sprite;
      [SerializeField] [TextArea] private string description;
      
      public string Description => description;

      public Data Data => data;

      public Sprite Sprite => sprite;

      public Properties Properties => properties;
   }
}
