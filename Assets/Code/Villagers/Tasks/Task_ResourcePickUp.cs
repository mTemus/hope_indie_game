using System;
using System.Collections.Generic;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum Task_ResourcePickUp_State 
    {
        GET_STORAGE,
        GO_TO_STORAGE,
        GET_RESOURCES_DATA,
        GO_TO_RESOURCE,
        COLLECT_RESOURCE,
        DELIVER_RESOURCE
    }
    
    public class Task_ResourcePickUp : Task
    {
        private Queue<ResourceToPickUp> resources = new Queue<ResourceToPickUp>();

        private Task_ResourcePickUp_State currentPickupResourcePickUpState;
        private ResourceToPickUp currentResource;
        private Warehouse warehouse;

        private int resourceAmount;

        public ResourceType StoredResourceType { get; }
        public bool HasWorker => worker != null;
        public bool CanStoreResources => resourceAmount < worker.Profession.Data.ResourceCarryingLimit;
        public bool IsResourceInDelivery =>
            currentPickupResourcePickUpState == Task_ResourcePickUp_State.DELIVER_RESOURCE || 
            currentPickupResourcePickUpState == Task_ResourcePickUp_State.GO_TO_STORAGE && worker.Profession.IsCarryingResource;
        
        public Task_ResourcePickUp(ResourceType storedResourceType)
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
                currentPickupResourcePickUpState = Task_ResourcePickUp_State.GO_TO_RESOURCE;
            }
            else {
                if (worker.Profession.IsCarryingResource) 
                    currentPickupResourcePickUpState = Task_ResourcePickUp_State.GO_TO_STORAGE;
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

        public override void Start()
        {
            switch (state) {
                case TaskState.NEW:
                    warehouse = worker.Profession.Workplace as Warehouse;
                    
                    if (warehouse == null) 
                        throw new Exception("CAN'T GET WAREHOUSE REFERENCE FROM WORKER " + worker.name + " IN TASK " + GetType());

                    currentPickupResourcePickUpState = Task_ResourcePickUp_State.GET_RESOURCES_DATA;
                    break;
                
                case TaskState.WAITING:
                    break;
                case TaskState.RUNNING:
                    break;
                case TaskState.INTERRUPTED:
                    break;
                case TaskState.PAUSED:
                    break;
                case TaskState.ABANDONED:
                    break;
                case TaskState.COMPLETED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public override void Execute()
        {
            state = TaskState.RUNNING;
            
            switch (currentPickupResourcePickUpState) {
                case Task_ResourcePickUp_State.GO_TO_STORAGE:
                    if (worker.MoveTo(warehouse.PivotedPosition)) 
                        currentPickupResourcePickUpState = worker.Profession.CarriedResource != null ? 
                            Task_ResourcePickUp_State.DELIVER_RESOURCE : Task_ResourcePickUp_State.GET_RESOURCES_DATA;
                    break;
                
                case Task_ResourcePickUp_State.GET_RESOURCES_DATA:
                    warehouse.GetResourcesToPickUpByType(this);
                    GetNextResource();
                    currentPickupResourcePickUpState = Task_ResourcePickUp_State.GO_TO_RESOURCE;
                    break;
                
                case Task_ResourcePickUp_State.GO_TO_RESOURCE:
                    if (worker.MoveTo(currentResource.transform.position))
                        currentPickupResourcePickUpState = Task_ResourcePickUp_State.COLLECT_RESOURCE;
                    break;
                
                case Task_ResourcePickUp_State.COLLECT_RESOURCE:
                    worker.UI.SetResourceIcon(currentResource.StoredResource.Type);
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
                
                case Task_ResourcePickUp_State.DELIVER_RESOURCE:
                    warehouse.StoreResource(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    worker.Profession.CarriedResource = null;
                    onTaskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("NO SUCH STATE FOR RESOURCE PICK UP TASK");
            }
        }
        
        public override void End() {}
        public override void Pause() {}
    }
}
