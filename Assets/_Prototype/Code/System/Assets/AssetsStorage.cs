using System;
using System.Linq;
using _Prototype.Code.Characters.Villagers.Professions;
using _Prototype.Code.World.Resources;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ThirdParty.LeanTween.Framework;
using UnityEngine;
using Data = _Prototype.Code.World.Resources.ResourceToGather.Data;

//TODO: whole class should be deleted and all assets should be available through addressables or there should be created a database in scriptable objects
namespace _Prototype.Code.System.Assets
{
   /// <summary>
   /// 
   /// </summary>
   public class AssetsStorage : MonoBehaviour
   {
      [Header("Villagers")] 
      [SerializeField] private GameObject[] villagerPrefabs;
      [SerializeField] private Characters.Villagers.Professions.Data[] professionData;

      [Header("Buildings")] 
      [SerializeField] private GameObject[] buildingPrefabs;

      [Header("Resources")] 
      [SerializeField] private Data[] resourcesToGatherData;
      
      [Header("Sprites")]
      [SerializeField] private Sprite[] resourceIcons;

      [Header("Behaviour Trees")] 
      [SerializeField] private Blackboard[] blackboards;
      [SerializeField] private BehaviourTree[] behaviourTrees;

      [Header("Sounds")] 
      [SerializeField] private Sound[] walkingSoundEffects;
      [SerializeField] private Sound[] constructionSoundEffects;


      [Header("Other")] 
      [SerializeField] private GameObject resourceOnGround;
      
      /// <summary>
      /// 
      /// </summary>
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
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="resourceType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public Sprite GetResourceIcon(ResourceType resourceType)
      {
         Sprite s = resourceIcons.FirstOrDefault(sprite => sprite.name == resourceType.ToString().ToLower());

         if (s == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND SPRITE FOR RESOURCE: " + resourceType);
         
         return s;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="villagerType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public GameObject GetVillagerPrefab(ProfessionType villagerType)
      {
         GameObject p = villagerPrefabs.FirstOrDefault(villager => villager.name.Contains(villagerType.ToString()));
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + villagerType);

         return p;
      }
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="buildingName"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public GameObject GetBuildingPrefab(string buildingName)
      {
         GameObject p = buildingPrefabs.FirstOrDefault( building => building.name == buildingName);
         
         if (p == null) 
            throw new Exception("ASSET STORAGE ----- CAN'T FIND PREFAB FOR VILLAGER: " + buildingName);

         return p;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="professionType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public Characters.Villagers.Professions.Data GetProfessionDataForProfessionType(ProfessionType professionType)
      {
         Characters.Villagers.Professions.Data p = professionData.FirstOrDefault(data => data.Type == professionType);

         if (p == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND DATA FOR PROFESSION: " + professionType);
         
         return p;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="resourceType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public Data GetResourceToGatherDataByResourceType(ResourceType resourceType)
      {
         Data r =
            resourcesToGatherData.FirstOrDefault(resource => resource.ResourceType == resourceType);
         
         if (r == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND DATA FOR PROFESSION: " + resourceType);

         return r;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="aiType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public BehaviourTree GetBehaviourTreeForAIType(ProfessionAIType aiType)
      {
         BehaviourTree bt = behaviourTrees.FirstOrDefault(tree => tree.name.Contains(aiType.ToString()));

         if (bt == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND BT FOR AI TYPE: " + aiType);

         return bt;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="aiType"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public Blackboard GetBlackboardForAIType(ProfessionAIType aiType)
      {
         Blackboard blackboard = blackboards.FirstOrDefault(board => board.name.Contains(aiType.ToString()));

         if (blackboard == null)
            throw new Exception("ASSET STORAGE ----- CAN'T FIND BLACKBOARD FOR AI TYPE: " + aiType);

         return blackboard;
      }
      
      
      //TODO: these should be added somewhere else
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="resource"></param>
      /// <param name="mapX"></param>
      public void ThrowResourceOnTheGround(Resource resource, float mapX)
      {
         Characters.Villagers.Professions.Data haulerData = GetProfessionDataForProfessionType(ProfessionType.GlobalHauler);
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

      /// <summary>
      /// 
      /// </summary>
      /// <param name="soundType"></param>
      /// <param name="assetName"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public AudioClip[] GetAudioClipsByName(SoundType soundType, string assetName)
      {
         AudioClip[] clips = null;

         switch (soundType) {
            case SoundType.Walking:
               break;
            
            case SoundType.Background:
               break;
            
            case SoundType.Construction:
               clips = GetConstructionAudioClipsByName(assetName);
               break;
            
            default:
               throw new Exception("ASSET STORAGE ----- CAN'T FIND SOUND ASSETS FOR TYPE: " + soundType);
         }
         
         return clips;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="soundType"></param>
      /// <param name="assetName"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentOutOfRangeException"></exception>
      public AudioClip GetAudioClipByName(SoundType soundType, string assetName)
      {
         AudioClip clip = null;
         
         switch (soundType) {
            case SoundType.Walking:
               clip = GetWalkingAudioClipByAreaName(assetName);
               break;
            
            case SoundType.Background:
               break;
            
            case SoundType.Construction:
               break;
            
            default:
               throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
         }

         return clip;
      }
   }
}
