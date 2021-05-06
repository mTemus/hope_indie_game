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

        private Vector3 fromStoragePosition;
        private ResourceCarryingTaskState resourceCarryingState;
        
        private bool IsResourceInDelivery =>
            resourceCarryingState == ResourceCarryingTaskState.DELIVER_RESOURCES ||
            resourceCarryingState == ResourceCarryingTaskState.GO_TO_STORAGE && worker.Profession.IsCarryingResource;
        
        public Func<ResourceType, int, Resource> onResourceWithdraw;
        public Func<Task, int, Resource> onReservedResourceWithdraw;
        public Action<Resource> onResourceDelivery;

        public Resource ResourceToCarry => resourceToCarry;

        private ResourceCarryingTask(Resource resourceToCarry, Building toStorage)
        {
            this.resourceToCarry = resourceToCarry;
            
            reservedResources = false;
            taskPosition = toStorage.PivotedPosition;
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }

        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, Building fromStorage, bool reservedResources)
            : this(resourceToCarry, toStorage)
        {
            this.reservedResources = reservedResources;
            fromStoragePosition = fromStorage.PivotedPosition;
            resourceCarryingState = reservedResources ? ResourceCarryingTaskState.FIND_CLOSEST_STORAGE : ResourceCarryingTaskState.GO_TO_STORAGE;
        }
        
        public ResourceCarryingTask(Resource resourceToCarry, Building toStorage, bool reservedResources)
            : this(resourceToCarry, toStorage)
        {
            this.reservedResources = reservedResources;
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }

        public override void StartTask()
        {
           
        }

        public override void EndTask()
        {
            
        }

        public override void DoTask()
        {
            switch (resourceCarryingState) {
                case ResourceCarryingTaskState.FIND_CLOSEST_STORAGE:
                    Building fromStorage = Managers.I.Buildings
                        .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), taskPosition);
                    fromStoragePosition = fromStorage.PivotedPosition;

                    if (reservedResources) 
                        onReservedResourceWithdraw += Warehouse.GetReservedResource;
                    else 
                        onResourceWithdraw += fromStorage.Storage.WithdrawResource;
                    
                    resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    break;
                
                case ResourceCarryingTaskState.GO_TO_STORAGE:
                    if (!worker.MoveTo(fromStoragePosition))
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
                    if (!worker.MoveTo(taskPosition))
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

        public override void PauseTask()
        {
            
        }

        public override void AbandonTask()
        {
            if (IsResourceInDelivery) {
                resourceToCarry.amount += worker.Profession.CarriedResource.amount;
                ThrowResourceOnGround();
            }
            
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }
    }
}
