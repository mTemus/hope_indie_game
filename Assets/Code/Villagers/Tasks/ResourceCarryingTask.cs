using System;
using Code.Map.Building;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Resources;
using Code.System;
using Code.System.Properties;
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
        private readonly Resource requiredResource;
        private ResourceCarryingTaskState resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;

        private readonly bool reservedResources;
        
        private Building fromStorage;
        private Vector3 fromStoragePosition;

        private readonly Func<ResourceType, int, Resource> onResourceWithdraw;
        private readonly Func<Task, int, Resource> onReservedResourceWithdraw;
        private readonly Action<Resource> onResourceDelivery;

        public ResourceCarryingTask(Resource requiredResource, Building toStorage, Action<Resource> onResourceDelivery, Func<ResourceType, int, Resource> onResourceWithdraw, Building fromStorage = null)
        {
            this.requiredResource = requiredResource;
            this.fromStorage = fromStorage;
            this.onResourceDelivery = onResourceDelivery;
            this.onResourceWithdraw = onResourceWithdraw;

            reservedResources = false;
            taskPosition = toStorage.PivotedPosition;

            if (fromStorage != null) fromStoragePosition = fromStorage.PivotedPosition;
        }
        
        public ResourceCarryingTask(Resource requiredResource, Building toStorage, Action<Resource> onResourceDelivery, Func<Task, int, Resource> onReservedResourceWithdraw, Building fromStorage = null)
        {
            this.requiredResource = requiredResource;
            this.fromStorage = fromStorage;
            this.onResourceDelivery = onResourceDelivery;
            this.onReservedResourceWithdraw = onReservedResourceWithdraw;

            reservedResources = true;
            taskPosition = toStorage.PivotedPosition;
            
            if (fromStorage != null) fromStoragePosition = fromStorage.PivotedPosition;
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
                        typeof(Warehouse), worker.transform.position);
                    fromStoragePosition = fromStorage.PivotedPosition;
                    break;
                
                case ResourceCarryingTaskState.GO_TO_STORAGE:
                    worker.MoveTo(fromStoragePosition);
                    
                    if (Vector3.Distance(worker.transform.position, fromStoragePosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.TAKE_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.TAKE_RESOURCES:
                    // Worker.Profession.CarriedResource = storage.GetReservedResource(this, requiredResource.amount > GlobalProperties.MAXResourceHeld ? 
                    //     GlobalProperties.MAXResourceHeld : requiredResource.amount);

                    if (reservedResources) {
                        worker.Profession.CarriedResource = onReservedResourceWithdraw.Invoke(this,
                            requiredResource.amount > GlobalProperties.MAXResourceHeld
                                ? GlobalProperties.MAXResourceHeld
                                : requiredResource.amount);
                    }
                    else {
                        worker.Profession.CarriedResource = onResourceWithdraw.Invoke(
                            requiredResource.Type,
                            requiredResource.amount > GlobalProperties.MAXResourceHeld
                                ? GlobalProperties.MAXResourceHeld
                                : requiredResource.amount);
                    }
                    
                    worker.UI.SetResourceIcon(true, requiredResource.Type);
                    requiredResource.amount -= worker.Profession.CarriedResource.amount;
                    resourceCarryingState = ResourceCarryingTaskState.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingTaskState.GO_ON_TASK_POSITION:
                    worker.MoveTo(taskPosition);
                    if (Vector3.Distance(worker.transform.position, taskPosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.DELIVER_RESOURCES:
                    onResourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false, requiredResource.Type);

                    if (requiredResource.amount != 0) 
                        resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    else {
                        onTaskCompleted.Invoke();
                    }
                    break;
            }
        }

        public override void OnTaskPause()
        {
            
        }
    }
}
