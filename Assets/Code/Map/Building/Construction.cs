using System.Collections.Generic;
using System.Linq;
using Code.Map.Building.Buildings.Components;
using Code.Resources;
using Code.System;
using Code.System.Properties;
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

        private bool AreResourceDelivered() =>
            requiredResources.All(resource => resource.amount == 0);
        
        public bool Construct()
        {
            currentProgress += 1 * Time.deltaTime;
            currentProgress = Mathf.Clamp(currentProgress, 0f, 5f);
            return currentProgress >= 5f;
        }

        public void SetRequiredResource(Resource resource)
        {
            requiredResources.Add(resource);
        }

        public void SetBuildingTask(BuildingTask thisTask)
        {
            buildingTask = thisTask;
        }
        
        public Resource AddResources(Resource deliveredResource)
        {
            Resource res = requiredResources.Single(resource => resource.Type == deliveredResource.Type);
            res.amount = Mathf.Max(0, res.amount - deliveredResource.amount);
            Resource resourceLeft = new Resource(deliveredResource.Type);

            if (AreResourceDelivered())
                buildingTask.ResourcesDelivered = true;
            else 
                resourceLeft.amount = res.amount > GlobalProperties.MAXResourceHeld ? 
                GlobalProperties.MAXResourceHeld : res.amount;
            return resourceLeft;
        }

        public void InitializeConstruction(BuildingScript buildingData)
        {
            positionOffset = new Vector3(buildingData.xPivot, buildingData.yPivot, 0f);
            Warehouse warehouse = Managers.Instance.Buildings.GetClosestWarehouse();
            GetComponent<Building>().SetEntrancePivot(positionOffset);
            
            Managers.Instance.Tasks.CreateBuildingTask(this);
            
            foreach (Resource resource in buildingData.RequiredResources) {
                SetRequiredResource(resource);
                Managers.Instance.Tasks.CreateResourceCarryingTask(transform.position + positionOffset, ProfessionType.BUILDER, warehouse, resource, AddResources);
            }
        }

        public void CleanAfterConstruction()
        {
            DestroyImmediate(GetComponent<Construction>());
        }

        public Vector3 PositionOffset => positionOffset;
    }
}
