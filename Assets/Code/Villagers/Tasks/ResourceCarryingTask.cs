using System;
using Code.Map.Building;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using Code.System;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum ResourceCarryingTaskState
    {
        FIND_CLOSEST_STORAGE,
        GO_TO_STORAGE, 
        TAKE_RESOURCES,
        GO_ON_TASK_POSITION,
        DELIVER_RESOURCES
    }

    public class ResourceCarryingTask : Task
    {
        private readonly Resource resourceToCarry;
        private readonly bool reservedResources;

        private Building fromStorage;
        private Vector3 fromStoragePosition;
        private ResourceCarryingTaskState resourceCarryingState;
        
        private Func<ResourceType, int, Resource> onResourceWithdraw;
        private Func<Task, int, Resource> onReservedResourceWithdraw;
        private readonly Action<Resource> onResourceDelivery;
        
        private bool isResourceInDelivery =>
            resourceCarryingState == ResourceCarryingTaskState.DELIVER_RESOURCES ||
            resourceCarryingState == ResourceCarryingTaskState.GO_TO_STORAGE && worker.Profession.CarriedResource != null;
        
        public Resource ResourceToCarry => resourceToCarry;

        private ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Action<Resource> onResourceDelivery)
        {
            this.resourceToCarry = resourceToCarry;
            this.onResourceDelivery = onResourceDelivery;
            
            reservedResources = false;
            taskPosition = toStorage.PivotedPosition;
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }
        
        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Action<Resource> onResourceDelivery, Func<ResourceType, int, Resource> onResourceWithdraw, Building fromStorage) 
            : this(resourceToCarry, toStorage, onResourceDelivery)
        {
            this.fromStorage = fromStorage;
            this.onResourceWithdraw = onResourceWithdraw;

            reservedResources = false;
            fromStoragePosition = fromStorage.PivotedPosition;
            resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
        }
        
        //TODO: delete it <- it's for carrying up resources from ground, but they need own task type
        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Action<Resource> onResourceDelivery, Func<ResourceType, int, Resource> onResourceWithdraw, Vector3 fromStoragePosition) 
            : this(resourceToCarry, toStorage, onResourceDelivery)
        {
            this.onResourceWithdraw = onResourceWithdraw;

            reservedResources = false;
            this.fromStoragePosition = fromStoragePosition;
            resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
        }
        
        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Action<Resource> onResourceDelivery, Func<Task, int, Resource> onReservedResourceWithdraw, Building fromStorage)
            : this(resourceToCarry, toStorage, onResourceDelivery)
        {
            this.fromStorage = fromStorage;
            this.onReservedResourceWithdraw = onReservedResourceWithdraw;

            reservedResources = true;
            fromStoragePosition = fromStorage.PivotedPosition;
            resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
        }
        
        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Action<Resource> onResourceDelivery, bool reservedResources)
            : this(resourceToCarry, toStorage, onResourceDelivery)
        {
            this.reservedResources = reservedResources;
        }

        public override void OnTaskStart()
        {
           
        }

        public override void OnTaskEnd()
        {
            
        }

        public override void DoTask()
        {
            switch (resourceCarryingState) {
                case ResourceCarryingTaskState.FIND_CLOSEST_STORAGE:
                    resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    
                    if (fromStorage != null) break;
                    fromStorage = Managers.Instance.Buildings.GetClosestBuildingOfClass(BuildingType.Resources,
                        typeof(Warehouse), taskPosition);
                    fromStoragePosition = fromStorage.PivotedPosition;

                    if (reservedResources) {
                        onReservedResourceWithdraw = Warehouse.GetReservedResource;
                    }
                    else {
                        onResourceWithdraw = fromStorage.Storage.WithdrawResource;
                    }
                    break;
                
                case ResourceCarryingTaskState.GO_TO_STORAGE:
                    worker.MoveTo(fromStoragePosition);
                    
                    if (Vector3.Distance(worker.transform.position, fromStoragePosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.TAKE_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.TAKE_RESOURCES:
                    int currResAmount = resourceToCarry.amount;
                    int maxResourceAmount = worker.Profession.Data.ResourceCarryingLimit;
                
                    if (reservedResources) {
                        worker.Profession.CarriedResource = onReservedResourceWithdraw.Invoke(this,
                            currResAmount > maxResourceAmount ? maxResourceAmount : currResAmount);
                    }
                    else {
                        worker.Profession.CarriedResource = onResourceWithdraw.Invoke(
                            resourceToCarry.Type,
                            currResAmount > maxResourceAmount ? maxResourceAmount : currResAmount) ;
                    }
                    
                    worker.UI.SetResourceIcon(true, resourceToCarry.Type);
                    resourceToCarry.amount -= worker.Profession.CarriedResource.amount;
                    resourceCarryingState = ResourceCarryingTaskState.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingTaskState.GO_ON_TASK_POSITION:
                    worker.MoveTo(taskPosition);
                    if (Vector3.Distance(worker.transform.position, taskPosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.DELIVER_RESOURCES:
                    onResourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false, resourceToCarry.Type);
                    Debug.LogWarning(worker.name + " has delivered: " + worker.Profession.CarriedResource.amount + " " + worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;

                    if (resourceToCarry.amount != 0) 
                        resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    else 
                        onTaskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("TASK CARRYING STATE NOT SET");
            }

            worker.UI.StateText.text = "Resource carrying: " + resourceCarryingState;
        }

        public override void OnTaskPause()
        {
            
        }

        public override void OnTaskAbandon()
        {
            if (isResourceInDelivery) {
                resourceToCarry.amount += worker.Profession.CarriedResource.amount;
                ThrowResourceOnGround();
            }
            
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }
    }
}
