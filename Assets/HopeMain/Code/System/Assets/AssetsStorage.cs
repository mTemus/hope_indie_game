using System;
using System.Linq;
using HopeMain.Code.Characters.Villagers.Profession;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ThirdParty.LeanTween.Framework;
using UnityEngine;

//TODO: whole class should be deleted and all assets should be available through addressables
namespace HopeMain.Code.System.Assets
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

      [Header("Sounds")] 
      [SerializeField] private Asset_Sound[] walkingSoundEffects;
      [SerializeField] private Asset_Sound[] constructionSoundEffects;


      [Header("Other")] 
      [SerializeField] private GameObject resourceOnGround;
      
      public static AssetsStorage I { get; private set; }
   
      private void Awake()
      {
         I = this;
         LeanTween.init(100);
      }

      private AudioClip[] GetConstructionAudioClipsByName(string clipName) =>
         constructionSoundEffects
            .Where(sound => sound.AssetName.Contains(clipName))
            .Select(asset => asset.Clip)
            .ToArray();

      private AudioClip GetWalkingAudioClipByAreaName(string areaName) =>
         walkingSoundEffects
            .SingleOrDefault(sound => sound.AssetName.Contains(areaName))
            ?.Clip;
      
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

      public AudioClip[] GetAudioClipsByName(AssetSoundType soundType, string assetName)
      {
         AudioClip[] clips = null;

         switch (soundType) {
            case AssetSoundType.Walking:
               break;
            
            case AssetSoundType.Background:
               break;
            
            case AssetSoundType.Construction:
               clips = GetConstructionAudioClipsByName(assetName);
               break;
            
            default:
               throw new Exception("ASSET STORAGE ----- CAN'T FIND SOUND ASSETS FOR TYPE: " + soundType);
         }
         
         return clips;
      }

      public AudioClip GetAudioClipByName(AssetSoundType soundType, string assetName)
      {
         AudioClip clip = null;
         
         switch (soundType) {
            case AssetSoundType.Walking:
               clip = GetWalkingAudioClipByAreaName(assetName);
               break;
            
            case AssetSoundType.Background:
               break;
            
            case AssetSoundType.Construction:
               break;
            
            default:
               throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
         }

         return clip;
      }
   }
}
