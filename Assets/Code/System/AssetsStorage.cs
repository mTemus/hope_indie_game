using System;
using System.Linq;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.Villagers.Professions;
using UnityEngine;

//TODO: whole class should be deleted and all assets should be available through adressables
namespace Code.System
{
   public class AssetsStorage : MonoBehaviour
   {
      [Header("Villagers")] 
      [SerializeField] private GameObject[] villagerPrefabs;
      [SerializeField] private ProfessionData[] professionData;

      [Header("Buildings")] 
      [SerializeField] private GameObject[] buildingPrefabs;

      [Header("Resources")] 
      [SerializeField] private ResourceToGatherData[] resourcesToGatherData;
      
      [Header("Sprites")]
      [SerializeField] private Sprite[] resourceIcons;
   
      public static AssetsStorage I { get; private set; }
   
      private void Awake()
      {
         I = this;
      }
   
      public Sprite GetResourceIcon(ResourceType resourceType)
      {
         Sprite s = resourceIcons.FirstOrDefault(sprite => sprite.name == resourceType.ToString().ToLower());

         if (s == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND SPRITE FOR RESOURCE: " + resourceType);
         
         return s;
      }

      public GameObject GetVillagerPrefab(ProfessionType villagerType)
      {
         GameObject p = villagerPrefabs.FirstOrDefault(villager => villager.name == villagerType.ToString());
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + villagerType);

         return p;
      }
      
      public GameObject GetBuildingPrefab(string buildingName)
      {
         GameObject p = buildingPrefabs.FirstOrDefault( building => building.name == buildingName);
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + buildingName);

         return p;
      }

      public ProfessionData GetProfessionDataForProfessionType(ProfessionType professionType)
      {
         ProfessionData p = professionData.FirstOrDefault(data => data.Type == professionType);

         if (p == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND DATA FOR PROFESSION: " + professionType);
         
         return p;
      }

      public ResourceToGatherData GetResourceToGatherDataByResourceType(ResourceType resourceType)
      {
         ResourceToGatherData r =
            resourcesToGatherData.FirstOrDefault(resource => resource.ResourceType == resourceType);
         
         if (r == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND DATA FOR PROFESSION: " + resourceType);

         return r;
      }
      
   }
}
