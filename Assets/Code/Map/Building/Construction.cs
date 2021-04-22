using System.Collections.Generic;
using System.Linq;
using Code.Map.Building.Buildings.Types.Industry;
using Code.Resources;
using Code.System;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building
{
    public class Construction : MonoBehaviour
    {
        private readonly List<Resource> requiredResources = new List<Resource>();
        private float currentProgress;
        private BuildingTask buildingTask;
        private Vector3 positionOffset;

        private Material normalMaterial;
        private Material buildingMaterial;
        private static readonly int Visibility = Shader.PropertyToID("Vector1_Visibility");

        private bool AreResourceDelivered() =>
            requiredResources.All(resource => resource.amount == 0);
        
        public bool Construct()
        {
            currentProgress -= 5 * Time.deltaTime;
            currentProgress = Mathf.Clamp(currentProgress, 0.1f, 30f);
            buildingMaterial.SetFloat(Visibility, currentProgress);

            if (!(currentProgress <= 0.1f)) return false;
            DestroyImmediate(buildingMaterial);
            GetComponent<SpriteRenderer>().material = normalMaterial;
            CleanAfterConstruction();
            return true;
        }

        public void SetRequiredResource(Resource resource)
        {
            requiredResources.Add(resource);
        }

        public void SetBuildingTask(BuildingTask thisTask)
        {
            buildingTask = thisTask;
        }
        
        public void AddResources(Resource deliveredResource)
        {
            Resource res = requiredResources.Single(resource => resource.Type == deliveredResource.Type);
            res.amount = Mathf.Max(0, res.amount - deliveredResource.amount);

            Debug.Log("Add resources of type " + res.Type +" to construction of " + name + ". Required: " + res.amount);
            
            if (AreResourceDelivered()) {
                Debug.LogError("Resources delivered for: " + name);
                buildingTask.SetResourcesAsDelivered();
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
            
            BuildersGuild buildersGuild = (BuildersGuild) Managers.Instance.Buildings.GetClosestBuildingOfClass(BuildingType.Industry,
                typeof(BuildersGuild), transform.position);
            
            buildersGuild.CreateBuildingTask(this, buildingData);
            
            // Managers.Instance.Tasks.CreateBuildingTask(this);
            
            foreach (Resource resource in buildingData.RequiredResources) {
                Resource requiredResource = new Resource(resource);
                SetRequiredResource(requiredResource);
                
                // ResourceCarryingTask rct = new ResourceCarryingTask(ProfessionType.Builder, transform.position + positionOffset, requiredResource, AddResources);
                // Managers.Instance.Resources.ReserveResources(rct, requiredResource);
            }

            GetComponent<Building>().Data = buildingData;
        }

        public void CleanAfterConstruction()
        {
            DestroyImmediate(GetComponent<Construction>());
        }

        public Vector3 PositionOffset => positionOffset;
    }
}
