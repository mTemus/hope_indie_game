using System.Collections.Generic;
using System.Linq;
using Code.Map.Building.Buildings.Types.Industry;
using Code.Map.Resources;
using Code.System;
using Code.System.Assets;
using Code.System.Initialization;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building
{
    public class Construction : MonoBehaviour
    {
        private readonly List<Resource> requiredResources = new List<Resource>();
        
        private float currentProgress;
        
        private Task_Building taskBuilding;
        private Vector3 positionOffset;
        private Material normalMaterial;
        private Material buildingMaterial;
        private AudioSource constructionChannel;

        private AudioClip[] clips;
        
        private static readonly int Visibility = Shader.PropertyToID("Vector1_Visibility");

        private bool AreResourceDelivered =>
            requiredResources.All(resource => resource.amount == 0);

        private void PlayConstructionSound()
        {
            if (constructionChannel.isPlaying) return;

            int idx = Random.Range(0, clips.Length);
            
            constructionChannel.clip = clips[idx];
            constructionChannel.Play();
        }
        
        public bool Construct()
        {
            PlayConstructionSound();
            currentProgress -= 5 * Time.deltaTime;
            currentProgress = Mathf.Clamp(currentProgress, 0.1f, 30f);
            buildingMaterial.SetFloat(Visibility, currentProgress);

            if (!(currentProgress <= 0.1f)) return false;
            GetComponent<SpriteRenderer>().material = normalMaterial;
            GetComponent<InitializeBuilding>().InitializeMe();
            return true;
        }

        public void SetRequiredResource(Resource resource)
        {
            requiredResources.Add(resource);
        }

        public void SetBuildingTask(Task_Building @this)
        {
            taskBuilding = @this;
        }
        
        public void AddResources(Resource deliveredResource)
        {
            Resource res = requiredResources.Single(resource => resource.Type == deliveredResource.Type);
            res.amount = Mathf.Max(0, res.amount - deliveredResource.amount);

            Debug.Log("Add resources of type " + res.Type +" to construction of " + name + ". Required: " + res.amount);
            
            if (AreResourceDelivered) {
                Debug.LogError("Resources delivered for: " + name);
                taskBuilding.SetResourcesAsDelivered();
            }
        }

        public void InitializeConstruction(BuildingData buildingData, Material fadeMaterial)
        {
            SpriteRenderer buildingRenderer = GetComponent<SpriteRenderer>();
            
            normalMaterial = buildingRenderer.material;
            buildingMaterial = fadeMaterial;
            buildingRenderer.material = buildingMaterial;
            
            currentProgress = buildingMaterial.GetFloat(Visibility);
            
            positionOffset = new Vector3(buildingData.EntrancePivot.x, buildingData.EntrancePivot.y, 0f);

            BuildersGuild buildersGuild = (BuildersGuild) Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Industry,
                typeof(BuildersGuild), transform.position);
            
            buildersGuild.CreateBuildingTask(this, buildingData);
            
            foreach (Resource resource in buildingData.RequiredResources) 
                SetRequiredResource(new Resource(resource));
            
            constructionChannel = gameObject.AddComponent<AudioSource>();
            constructionChannel.playOnAwake = false;
            constructionChannel.volume = 0.5f;
            constructionChannel.spatialBlend = 1f;
            constructionChannel.minDistance = 3f;
            constructionChannel.maxDistance = 10f;
            clips = AssetsStorage.I.GetAudioClipsByName(AssetSoundType.Construction, "construction");
        }

        public void CleanAfterConstruction()
        {
            DestroyImmediate(buildingMaterial);
            DestroyImmediate(constructionChannel);
            DestroyImmediate(GetComponent<Construction>());
        }

        public Vector3 PositionOffset => positionOffset;
    }
}
