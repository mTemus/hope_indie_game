using System;
using System.Linq;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.Villagers.Professions;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ThirdParty.LeanTween.Framework;
using UnityEngine;

//TODO: whole class should be deleted and all assets should be available through addressables
namespace Code.System
{
   public class AssetsStorage : MonoBehaviour
   {
      [Header("Villagers")] 
      [SerializeField] private GameObject[] villagerPrefabs;
      [SerializeField] private Villager_ProfessionData[] professionData;

      [Header("Buildings")] 
      [SerializeField] private GameObject[] buildingPrefabs;

      [Header("Resources")] 
      [SerializeField] private ResourceToGatherData[] resourcesToGatherData;
      
      [Header("Sprites")]
      [SerializeField] private Sprite[] resourceIcons;

      [Header("Behaviour Trees")] 
      [SerializeField] private Blackboard[] blackboards;
      [SerializeField] private BehaviourTree[] behaviourTrees;
      
      [Header("Other")] 
      [SerializeField] private GameObject resourceOnGround;
      
      public static AssetsStorage I { get; private set; }
   
      private void Awake()
      {
         I = this;
         LeanTween.init(100);
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
         GameObject p = villagerPrefabs.FirstOrDefault(villager => villager.name.Contains(villagerType.ToString()));
         
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

      public Villager_ProfessionData GetProfessionDataForProfessionType(ProfessionType professionType)
      {
         Villager_ProfessionData p = professionData.FirstOrDefault(data => data.Type == professionType);

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

      public BehaviourTree GetBehaviourTreeForAIType(ProfessionAIType aiType)
      {
         BehaviourTree bt = behaviourTrees.FirstOrDefault(tree => tree.name.Contains(aiType.ToString()));

         if (bt == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND BT FOR AI TYPE: " + aiType);

         return bt;
      }

      public Blackboard GetBlackboardForAIType(ProfessionAIType aiType)
      {
         Blackboard blackboard = blackboards.FirstOrDefault(board => board.name.Contains(aiType.ToString()));

         if (blackboard == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND BLACKBOARD FOR AI TYPE: " + aiType);

         return blackboard;
      }
      
      
      //TODO: these should be added somewhere else
      
      public void ThrowResourceOnTheGround(Resource resource, float mapX)
      {
         Villager_ProfessionData haulerData = GetProfessionDataForProfessionType(ProfessionType.GlobalHauler);
         int resourceCnt =  Mathf.FloorToInt(resource.amount / haulerData.ResourceCarryingLimit) + (resource.amount % haulerData.ResourceCarryingLimit > 0 ? 1 : 0);
         int currentAmount = resource.amount;
         
         for (int i = 0; i < resourceCnt; i++) {
            Resource r;
            if (currentAmount > haulerData.ResourceCarryingLimit ) {
               currentAmount = resource.amount - haulerData.ResourceCarryingLimit;
               resource.amount = currentAmount;
               r = new Resource(resource.Type, haulerData.ResourceCarryingLimit);
            }
            else 
               r = new Resource(resource.Type, resource.amount);
            
            Instantiate(resourceOnGround, new Vector3(mapX, 2f, 0f), Quaternion.identity)
               .GetComponent<ResourceToPickUp>()
               .Initialize(r, mapX);
         }
      }
   }
}
