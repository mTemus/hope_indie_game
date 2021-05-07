using System;
using System.Collections.Generic;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using UnityEngine;

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
        private Queue<ResourceToPickUp> resources = new Queue<ResourceToPickUp>();

        private ResourcePickUpTaskState currentPickupState;
        private ResourceToPickUp currentResource;
        private Warehouse warehouse;

        private int resourceAmount;

        public ResourceType StoredResourceType { get; }
        public bool HasWorker => worker != null;
        public bool CanStoreResources => resourceAmount < worker.Profession.Data.ResourceCarryingLimit;
        public bool IsResourceInDelivery =>
            currentPickupState == ResourcePickUpTaskState.DELIVER_RESOURCE || 
            currentPickupState == ResourcePickUpTaskState.GO_TO_STORAGE && worker.Profession.IsCarryingResource;
        
        public ResourcePickUpTask(ResourceType storedResourceType)
        {
            StoredResourceType = storedResourceType;
        }
        
        private void SortResources()
        {
            Vector3 workerPosition = worker.transform.position;
            List<ResourceToPickUp> resourcesList = new List<ResourceToPickUp>(resources);

            for (int i = 0; i < resourcesList.Count; i++) {
                for (int sort = 0; sort < resourcesList.Count - 1; sort++) {
                    float distanceToResource = Vector3.Distance(workerPosition, resourcesList[sort].transform.position);
                    float distanceToNextResource = Vector3.Distance(workerPosition, resourcesList[sort + 1].transform.position);

                    if (distanceToResource <= distanceToNextResource) continue;
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
                if (worker.Profession.IsCarryingResource) 
                    currentPickupState = ResourcePickUpTaskState.GO_TO_STORAGE;
                else 
                    onTaskCompleted.Invoke();
            }
        }

        public bool AddResourceToPickUp(ResourceToPickUp resource)
        {
            int newAmount = resourceAmount + resource.StoredResource.amount;
            if (newAmount > worker.Profession.Data.ResourceCarryingLimit)
                return false;
            
            resourceAmount = newAmount;
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

        public override void StartTask()
        {
            currentPickupState = ResourcePickUpTaskState.GET_STORAGE;
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
                    if (worker.MoveTo(warehouse.PivotedPosition)) 
                        currentPickupState = worker.Profession.CarriedResource != null ? 
                            ResourcePickUpTaskState.DELIVER_RESOURCE : ResourcePickUpTaskState.GET_RESOURCES_DATA;
                    break;
                
                case ResourcePickUpTaskState.GET_RESOURCES_DATA:
                    warehouse.GetResourcesToPickUp(this);
                    GetNextResource();
                    currentPickupState = ResourcePickUpTaskState.GO_TO_RESOURCE;
                    break;
                
                case ResourcePickUpTaskState.GO_TO_RESOURCE:
                    if (worker.MoveTo(currentResource.transform.position))
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
                    warehouse.StoreResource(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false,  worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;
                    onTaskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("NO SUCH STATE FOR RESOURCE PICK UP TASK");
            }
        }
        
        public override void EndTask() {}
        public override void PauseTask() {}
        public override void AbandonTask() { }
    }
}
