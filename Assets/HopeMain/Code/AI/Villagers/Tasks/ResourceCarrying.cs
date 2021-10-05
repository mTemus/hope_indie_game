using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Buildings.Type.Resources;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public enum ResourceCarryingFlag
    {
        FIND_CLOSEST_STORAGE,
        GO_TO_STORAGE, 
        TAKE_RESOURCES,
        GO_ON_TASK_POSITION,
        DELIVER_RESOURCES
    }

    public class ResourceCarrying : Task
    {
        private readonly Resource resourceToCarry;
        private readonly bool reservedResources;

        private Vector3 fromStoragePosition;
        private ResourceCarryingFlag _flag;
        
        private bool IsResourceInDelivery =>
            _flag == ResourceCarryingFlag.DELIVER_RESOURCES ||
            _flag == ResourceCarryingFlag.GO_TO_STORAGE && worker.Profession.IsCarryingResource;
        
        public Func<ResourceType, int, Resource> onResourceWithdraw;
        public Func<Task, int, Resource> onReservedResourceWithdraw;
        public Action<Resource> onResourceDelivery;

        public Resource ResourceToCarry => resourceToCarry;

        public ResourceCarrying(Resource resourceToCarry, bool reservedResources, World.Buildings.Building toStorage, World.Buildings.Building fromStorage = null)
        {
            this.resourceToCarry = resourceToCarry;
            this.reservedResources = reservedResources;

            taskPosition = toStorage.PivotedPosition;

            if (fromStorage != null) {
                fromStoragePosition = fromStorage.PivotedPosition;
                _flag = ResourceCarryingFlag.GO_TO_STORAGE;
            }
            else {
                _flag = ResourceCarryingFlag.FIND_CLOSEST_STORAGE;
            }
        }

        public override void Start()
        {
            worker.Brain.Animations.Turn(fromStoragePosition);
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;
            
            switch (_flag) {
                case ResourceCarryingFlag.FIND_CLOSEST_STORAGE:
                    World.Buildings.Building fromStorage = Managers.I.Buildings
                        .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), taskPosition);
                    fromStoragePosition = fromStorage.PivotedPosition;

                    if (reservedResources) 
                        onReservedResourceWithdraw += Warehouse.GetReservedResource;
                    else 
                        onResourceWithdraw += fromStorage.Storage.WithdrawResource;
                    
                    worker.Brain.Animations.Turn(fromStoragePosition);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
                    _flag = ResourceCarryingFlag.GO_TO_STORAGE;
                    break;
                
                case ResourceCarryingFlag.GO_TO_STORAGE:
                    if (worker.Brain.Motion.MoveTo(fromStoragePosition))
                        _flag = ResourceCarryingFlag.TAKE_RESOURCES;
                    break;
                
                case ResourceCarryingFlag.TAKE_RESOURCES:
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
                    _flag = ResourceCarryingFlag.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingFlag.GO_ON_TASK_POSITION:
                    if (worker.Brain.Motion.MoveTo(taskPosition))
                        _flag = ResourceCarryingFlag.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingFlag.DELIVER_RESOURCES:
                    onResourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    Debug.LogWarning(worker.name + " has delivered: " + worker.Profession.CarriedResource.amount + " " + worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;

                    if (resourceToCarry.amount != 0) 
                        _flag = ResourceCarryingFlag.GO_TO_STORAGE;
                    else 
                        onTaskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("TASK CARRYING STATE NOT SET");
            }

            worker.UI.StateText.text = "Resource carrying: " + _flag;
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
            
            _flag = ResourceCarryingFlag.FIND_CLOSEST_STORAGE;
            base.Abandon();
        }
    }
}
