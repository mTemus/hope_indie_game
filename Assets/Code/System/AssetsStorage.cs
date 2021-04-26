using System;
using Code.Resources;
using Code.Villagers.Professions;
using UnityEngine;

//TODO: whole class should be deleted and all assets should be available through adressables
namespace Code.System
{
   public class AssetsStorage : MonoBehaviour
   {
      [Header("Villagers")] 
      [SerializeField] private GameObject[] villagerPrefabs;

      [Header("Buildings")] 
      [SerializeField] private GameObject[] buildingPrefabs;
      
      [Header("Sprites")]
      [SerializeField] private Sprite[] resourceIcons;
   
      public static AssetsStorage I { get; private set; }
   
      private void Awake()
      {
         I = this;
      }
   
      public Sprite GetResourceIcon(ResourceType resourceType)
      {
         Sprite s = Array.Find(resourceIcons, sprite => sprite.name == resourceType.ToString().ToLower());

         if (s == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND SPRITE FOR RESOURCE: " + resourceType);
         
         return s;
      }

      public GameObject GetVillagerPrefab(ProfessionType villagerType)
      {
         GameObject p = Array.Find(villagerPrefabs, villager => villager.name == villagerType.ToString());
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + villagerType);

         return p;
      }
      
      public GameObject GetBuildingPrefab(string buildingName)
      {
         GameObject p = Array.Find(villagerPrefabs, building => building.name == buildingName);
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + buildingName);

         return p;
      }
      
   }
}
