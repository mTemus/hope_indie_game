using System;
using Code.AI.VillagerBrain.Layers;
using Code.Map.Building;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using Code.System;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum Task_ResourceCarrying_State
    {
        FIND_CLOSEST_STORAGE,
        GO_TO_STORAGE, 
        TAKE_RESOURCES,
        GO_ON_TASK_POSITION,
        DELIVER_RESOURCES
    }

    public class Task_ResourceCarrying : Task
    {
        private readonly Resource resourceToCarry;
        private readonly bool reservedResources;

        private Vector3 fromStoragePosition;
        private Task_ResourceCarrying_State taskResourceCarryingState;
        
        private bool IsResourceInDelivery =>
            taskResourceCarryingState == Task_ResourceCarrying_State.DELIVER_RESOURCES ||
            taskResourceCarryingState == Task_ResourceCarrying_State.GO_TO_STORAGE && worker.Profession.IsCarryingResource;
        
        public Func<ResourceType, int, Resource> onResourceWithdraw;
        public Func<Task, int, Resource> onReservedResourceWithdraw;
        public Action<Resource> onResourceDelivery;

        public Resource ResourceToCarry => resourceToCarry;

        public Task_ResourceCarrying(Resource resourceToCarry, bool reservedResources, Building toStorage, Building fromStorage = null)
        {
            this.resourceToCarry = resourceToCarry;
            this.reservedResources = reservedResources;

            taskPosition = toStorage.PivotedPosition;

            if (fromStorage != null) {
                fromStoragePosition = fromStorage.PivotedPosition;
                taskResourceCarryingState = Task_ResourceCarrying_State.GO_TO_STORAGE;
            }
            else {
                taskResourceCarryingState = Task_ResourceCarrying_State.FIND_CLOSEST_STORAGE;
            }
        }

        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;
            
            switch (taskResourceCarryingState) {
                case Task_ResourceCarrying_State.FIND_CLOSEST_STORAGE:
                    Building fromStorage = Managers.I.Buildings
                        .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), taskPosition);
                    fromStoragePosition = fromStorage.PivotedPosition;

                    if (reservedResources) 
                        onReservedResourceWithdraw += Warehouse.GetReservedResource;
                    else 
                        onResourceWithdraw += fromStorage.Storage.WithdrawResource;
                    
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
                    taskResourceCarryingState = Task_ResourceCarrying_State.GO_TO_STORAGE;
                    break;
                
                case Task_ResourceCarrying_State.GO_TO_STORAGE:
                    if (worker.Brain.Motion.MoveTo(fromStoragePosition))
                        taskResourceCarryingState = Task_ResourceCarrying_State.TAKE_RESOURCES;
                    break;
                
                case Task_ResourceCarrying_State.TAKE_RESOURCES:
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
                    
                    worker.UI.SetResourceIcon(resourceToCarry.Type);
                    resourceToCarry.amount -= worker.Profession.CarriedResource.amount;
                    taskResourceCarryingState = Task_ResourceCarrying_State.GO_ON_TASK_POSITION;
                    break;
                
                case Task_ResourceCarrying_State.GO_ON_TASK_POSITION:
                    if (worker.Brain.Motion.MoveTo(taskPosition))
                        taskResourceCarryingState = Task_ResourceCarrying_State.DELIVER_RESOURCES;
                    break;
                
                case Task_ResourceCarrying_State.DELIVER_RESOURCES:
                    onResourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    Debug.LogWarning(worker.name + " has delivered: " + worker.Profession.CarriedResource.amount + " " + worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;

                    if (resourceToCarry.amount != 0) 
                        taskResourceCarryingState = Task_ResourceCarrying_State.GO_TO_STORAGE;
                    else 
                        onTaskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("TASK CARRYING STATE NOT SET");
            }

            worker.UI.StateText.text = "Resource carrying: " + taskResourceCarryingState;
        }

        public override void End()
        {
            flag = TaskFlag.COMPLETED;
        }
        public override void Pause() {}

        public override void Abandon()
        {
            if (IsResourceInDelivery) {
                resourceToCarry.amount += worker.Profession.CarriedResource.amount;
                ThrowResourceOnGround();
            }
            
            taskResourceCarryingState = Task_ResourceCarrying_State.FIND_CLOSEST_STORAGE;
            base.Abandon();
        }
    }
}
