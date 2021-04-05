using System.Collections.Generic;
using System.Linq;
using Code.Resources;
using Code.System;
using Code.Villagers.Professions;
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
            
            if (AreResourceDelivered())
                buildingTask.ResourcesDelivered = true;
        }

        public void InitializeConstruction(BuildingData buildingData, Material fadeMaterial)
        {
            SpriteRenderer buildingRenderer = GetComponent<SpriteRenderer>();
            
            normalMaterial = buildingRenderer.material;
            buildingMaterial = fadeMaterial;
            buildingRenderer.material = buildingMaterial;
            
            currentProgress = buildingMaterial.GetFloat(Visibility);
            
            positionOffset = new Vector3(buildingData.XPivot, buildingData.YPivot, 0f);
            Warehouse warehouse = Managers.Instance.Buildings.GetClosestWarehouse();
            
            Managers.Instance.Tasks.CreateBuildingTask(this);
            
            foreach (Resource resource in buildingData.RequiredResources) {
                Resource requiredResource = new Resource(resource);
                SetRequiredResource(requiredResource);
                Managers.Instance.Tasks.CreateResourceCarryingTask(transform.position + positionOffset, ProfessionType.Builder, warehouse, requiredResource, AddResources);
            }

            Building b = GetComponent<Building>();
            b.SetBuildingSize(buildingData.Width, buildingData.Height);
            b.SetEntrancePivot(positionOffset);
            b.SetMaxOccupancy(buildingData.MAXOccupancy);
        }

        public void CleanAfterConstruction()
        {
            DestroyImmediate(GetComponent<Construction>());
        }

        public Vector3 PositionOffset => positionOffset;
    }
}
