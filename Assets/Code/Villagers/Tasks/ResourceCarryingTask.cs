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
        private readonly Resource carriedResource;
        private readonly bool reservedResources;

        private Building fromStorage;
        private Vector3 fromStoragePosition;
        private ResourceCarryingTaskState resourceCarryingState;
        
        private Func<ResourceType, int, Resource> onResourceWithdraw;
        private Func<Task, int, Resource> onReservedResourceWithdraw;
        private readonly Action<Resource> onResourceDelivery;

        public Resource CarriedResource => carriedResource;

        private ResourceCarryingTask(Resource carriedResource, Building toStorage, Action<Resource> onResourceDelivery)
        {
            this.carriedResource = carriedResource;
            this.onResourceDelivery = onResourceDelivery;
            
            reservedResources = false;
            taskPosition = toStorage.PivotedPosition;
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }
        
        public ResourceCarryingTask(Resource carriedResource, Building toStorage, Action<Resource> onResourceDelivery, Func<ResourceType, int, Resource> onResourceWithdraw, Building fromStorage) 
            : this(carriedResource, toStorage, onResourceDelivery)
        {
            this.fromStorage = fromStorage;
            this.onResourceWithdraw = onResourceWithdraw;

            reservedResources = false;
            fromStoragePosition = fromStorage.PivotedPosition;
        }
        
        public ResourceCarryingTask(Resource carriedResource, Building toStorage, Action<Resource> onResourceDelivery, Func<Task, int, Resource> onReservedResourceWithdraw, Building fromStorage)
            : this(carriedResource, toStorage, onResourceDelivery)
        {
            this.fromStorage = fromStorage;
            this.onReservedResourceWithdraw = onReservedResourceWithdraw;

            reservedResources = true;
            fromStoragePosition = fromStorage.PivotedPosition;
        }
        
        public ResourceCarryingTask(Resource carriedResource, Building toStorage, Action<Resource> onResourceDelivery, bool reservedResources)
            : this(carriedResource, toStorage, onResourceDelivery)
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
                        Warehouse warehouse = (Warehouse) fromStorage;
                        onReservedResourceWithdraw = warehouse.GetReservedResource;
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
                    if (reservedResources) {
                        worker.Profession.CarriedResource = onReservedResourceWithdraw.Invoke(this,
                            carriedResource.amount > worker.Profession.Data.ResourceCarryingLimit
                                ? worker.Profession.Data.ResourceCarryingLimit
                                : carriedResource.amount);
                    }
                    else {
                        worker.Profession.CarriedResource = onResourceWithdraw.Invoke(
                            carriedResource.Type,
                            carriedResource.amount > worker.Profession.Data.ResourceCarryingLimit
                                ? worker.Profession.Data.ResourceCarryingLimit
                                : carriedResource.amount);
                    }
                    
                    worker.UI.SetResourceIcon(true, carriedResource.Type);
                    carriedResource.amount -= worker.Profession.CarriedResource.amount;
                    resourceCarryingState = ResourceCarryingTaskState.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingTaskState.GO_ON_TASK_POSITION:
                    worker.MoveTo(taskPosition);
                    if (Vector3.Distance(worker.transform.position, taskPosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.DELIVER_RESOURCES:
                    onResourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false, carriedResource.Type);

                    if (carriedResource.amount != 0) 
                        resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    else {
                        onTaskCompleted.Invoke();
                    }
                    break;
            }

            worker.UI.StateText.text = "Resource carrying: " + resourceCarryingState;
        }

        public override void OnTaskPause()
        {
            
        }

        public override void OnTaskAbandon()
        {
            if (resourceCarryingState == ResourceCarryingTaskState.DELIVER_RESOURCES) 
                carriedResource.amount += worker.Profession.CarriedResource.amount;
            
            resourceCarryingState = ResourceCarryingTaskState.FIND_CLOSEST_STORAGE;
        }
    }
}
