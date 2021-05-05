using System;
using System.Collections.Generic;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Code.Villagers.Tasks
{
    public enum ResourcePickUpTaskState 
    {
        GET_STORAGE,
        GO_TO_STORAGE,
        GET_RESOURCES_DATA,
        GO_TO_RESOURCE,
        COLLECT_RESOURCE,
        DELIVER_RESOURCE
    }
    
    public class ResourcePickUpTask : Task
    {
        private ResourcePickUpTaskState currentPickupState = ResourcePickUpTaskState.GET_STORAGE;
        private Queue<ResourceToPickUp> resources = new Queue<ResourceToPickUp>();
        private int resourceAmount;

        private readonly ResourceType storedResourceType;
        private ResourceToPickUp currentResource;
        private Warehouse warehouse;
        
        public bool HasWorker => worker != null;
        public bool CanStoreResources => resourceAmount < worker.Profession.Data.ResourceCarryingLimit;
        
        public ResourcePickUpTask(ResourceType storedResourceType)
        {
            this.storedResourceType = storedResourceType;
        }
        
        public ResourceType StoredResourceType => storedResourceType;

        public bool IsResourceInDelivery =>
            currentPickupState == ResourcePickUpTaskState.DELIVER_RESOURCE || 
            currentPickupState == ResourcePickUpTaskState.GO_TO_STORAGE && worker.Profession.CarriedResource != null;
        
        private void SortResources()
        {
            Vector3 workerPosition = worker.transform.position;

            List<ResourceToPickUp> resourcesList = new List<ResourceToPickUp>(resources);

            for (int i = 0; i < resourcesList.Count; i++) {
                for (int sort = 0; sort < resourcesList.Count - 1; sort++) {
                    float distanceOne = Vector3.Distance(workerPosition, resourcesList[sort].transform.position);
                    float distanceTwo = Vector3.Distance(workerPosition, resourcesList[sort + 1].transform.position);

                    if (!(distanceOne > distanceTwo)) continue;
                    ResourceToPickUp tmp = resourcesList[sort + 1];
                    resourcesList[sort + 1] = resourcesList[sort];
                    resourcesList[sort + 1] = tmp;
                }
            }

            resources = new Queue<ResourceToPickUp>(resourcesList);
        }

        private void GetNextResource()
        {
            if (resources.Count > 0) {
                currentResource = resources.Dequeue();
                currentPickupState = ResourcePickUpTaskState.GO_TO_RESOURCE;
            }
            else {
                if (worker.Profession.CarriedResource != null) 
                    currentPickupState = ResourcePickUpTaskState.GO_TO_STORAGE;
                else 
                    worker.Profession.OnTaskCompleted();
            }
        }

        public bool AddResourceToPickUp(ResourceToPickUp resource)
        {
            if (resourceAmount + resource.StoredResource.amount > worker.Profession.Data.ResourceCarryingLimit)
                return false;
            
            resourceAmount += resource.StoredResource.amount;
            resources.Enqueue(resource);
            SortResources();
            return true;
        }
        
        public void RemoveResourceBeforePickUp(ResourceToPickUp resource)
        {
            if (currentResource == resource) 
                GetNextResource();
            
            resourceAmount -= resource.StoredResource.amount;
            List<ResourceToPickUp> resourcesList = new List<ResourceToPickUp>(resources);
            resourcesList.Remove(resource);
            resources = new Queue<ResourceToPickUp>(resourcesList);
        }

        public override void OnTaskStart()
        {
            
        }

        public override void OnTaskEnd()
        {
            
        }

        public override void DoTask()
        {
            switch (currentPickupState) {
                case ResourcePickUpTaskState.GET_STORAGE:
                    warehouse = worker.Profession.Workplace as Warehouse;
                    
                    if (warehouse == null) 
                        throw new Exception("CAN'T GET WAREHOUSE REFERENCE FROM WORKER " + worker.name + " IN TASK " + GetType());

                    currentPickupState = ResourcePickUpTaskState.GET_RESOURCES_DATA;
                    break;
                
                case ResourcePickUpTaskState.GO_TO_STORAGE:
                    Vector3 workplacePos = warehouse.PivotedPosition;
                    worker.MoveTo(workplacePos);

                    if (worker.transform.position == workplacePos) 
                        currentPickupState = worker.Profession.CarriedResource != null ? 
                            ResourcePickUpTaskState.DELIVER_RESOURCE : ResourcePickUpTaskState.GET_RESOURCES_DATA;
                    break;
                
                case ResourcePickUpTaskState.GET_RESOURCES_DATA:
                    warehouse.GetResourcesToPickUp(this);
                    GetNextResource();
                    currentPickupState = ResourcePickUpTaskState.GO_TO_RESOURCE;
                    break;
                
                case ResourcePickUpTaskState.GO_TO_RESOURCE:
                    Vector3 resourcePos = currentResource.transform.position;
                    
                    worker.MoveTo(resourcePos);

                    if (worker.transform.position == resourcePos)
                        currentPickupState = ResourcePickUpTaskState.COLLECT_RESOURCE;
                    break;
                
                case ResourcePickUpTaskState.COLLECT_RESOURCE:
                    worker.UI.SetResourceIcon(true, currentResource.StoredResource.Type);
                    Resource collectedResource = currentResource.WithdrawResource();
                    Resource workerResource = worker.Profession.CarriedResource;

                    if (workerResource == null) 
                        worker.Profession.CarriedResource = collectedResource;
                    else 
                        workerResource.amount += collectedResource.amount;
                    
                    Debug.Log("Pickuped: " + currentResource.StoredResource.amount + " " + currentResource.StoredResource.Type);
                    
                    warehouse.UnregisterResourceToPickUp(currentResource);
                    GetNextResource();
                    break;
                
                case ResourcePickUpTaskState.DELIVER_RESOURCE:
                    Warehouse warehouseDeliver = worker.Profession.Workplace as Warehouse;

                    if (warehouseDeliver != null) {
                        warehouseDeliver.StoreResource(worker.Profession.CarriedResource);
                        worker.Profession.CarriedResource = null;
                        worker.UI.SetResourceIcon(false, currentResource.StoredResource.Type);

                        worker.Profession.OnTaskCompleted();
                    }
                    else {
                        throw new Exception("NO WAREHOUSE TO STORE RESOURCES FOR TASK: " + GetType());
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnTaskPause()
        {
            throw new NotImplementedException();
        }

        public override void OnTaskAbandon()
        {
            throw new NotImplementedException();
        }
    }
}
