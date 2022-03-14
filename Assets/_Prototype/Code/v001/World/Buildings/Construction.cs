using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.System.Assets;
using _Prototype.Code.v001.System.Initialization;
using _Prototype.Code.v001.World.Buildings.Type.Industry;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings
{
    /// <summary>
    /// 
    /// </summary>
    public class Construction : MonoBehaviour
    {
        private readonly List<Resource> _requiredResources = new List<Resource>();
        
        private float _currentProgress;
        
        private AI.Villagers.Tasks.Building _buildingTask;
        private Vector3 _positionOffset;
        private Material _normalMaterial;
        private Material _buildingMaterial;
        private AudioSource _constructionChannel;

        private AudioClip[] _clips;
        
        private static readonly int Visibility = Shader.PropertyToID("Vector1_Visibility");

        private bool AreResourceDelivered =>
            _requiredResources.All(resource => resource.amount == 0);

        private void PlayConstructionSound()
        {
            if (_constructionChannel.isPlaying) return;

            int idx = Random.Range(0, _clips.Length);
            
            _constructionChannel.clip = _clips[idx];
            _constructionChannel.Play();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Construct()
        {
            PlayConstructionSound();
            _currentProgress -= 5 * Time.deltaTime;
            _currentProgress = Mathf.Clamp(_currentProgress, 0.1f, 30f);
            _buildingMaterial.SetFloat(Visibility, _currentProgress);

            if (!(_currentProgress <= 0.1f)) return false;
            GetComponent<SpriteRenderer>().material = _normalMaterial;
            GetComponent<InitializeBuilding>().InitializeMe();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        public void SetRequiredResource(Resource resource)
        {
            _requiredResources.Add(resource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        public void SetBuildingTask(AI.Villagers.Tasks.Building @this)
        {
            _buildingTask = @this;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveredResource"></param>
        public void AddResources(Resource deliveredResource)
        {
            Resource res = _requiredResources.Single(resource => resource.Type == deliveredResource.Type);
            res.amount = Mathf.Max(0, res.amount - deliveredResource.amount);

            Debug.Log("Add resources of type " + res.Type +" to construction of " + name + ". Required: " + res.amount);
            
            if (AreResourceDelivered) {
                Debug.LogError("Resources delivered for: " + name);
                _buildingTask.SetReady();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingData"></param>
        /// <param name="fadeMaterial"></param>
        public void InitializeConstruction(Data buildingData, Material fadeMaterial)
        {
            SpriteRenderer buildingRenderer = GetComponent<SpriteRenderer>();
            
            _normalMaterial = buildingRenderer.material;
            _buildingMaterial = fadeMaterial;
            buildingRenderer.material = _buildingMaterial;
            
            _currentProgress = _buildingMaterial.GetFloat(Visibility);
            
            _positionOffset = new Vector3(buildingData.EntrancePivot.x, buildingData.EntrancePivot.y, 0f);

            BuildersGuild buildersGuild = (BuildersGuild) Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Industry,
                typeof(BuildersGuild), transform.position);
            
            buildersGuild.CreateBuildingTask(this, buildingData);
            
            foreach (Resource resource in buildingData.RequiredResources) 
                SetRequiredResource(new Resource(resource));
            
            _constructionChannel = gameObject.AddComponent<AudioSource>();
            _constructionChannel.playOnAwake = false;
            _constructionChannel.volume = 0.5f;
            _constructionChannel.spatialBlend = 1f;
            _constructionChannel.minDistance = 3f;
            _constructionChannel.maxDistance = 10f;
            _clips = AssetsStorage.I.GetAudioClipsByName(SoundType.Construction, "construction");
        }

        /// <summary>
        /// 
        /// </summary>
        public void CleanAfterConstruction()
        {
            DestroyImmediate(_buildingMaterial);
            DestroyImmediate(_constructionChannel);
            DestroyImmediate(GetComponent<Construction>());
        }

        public Vector3 PositionOffset => _positionOffset;
    }
}
