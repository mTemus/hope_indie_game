using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Buildings.Type.Resources;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResourceCarryingFlag
    {
        FindClosestStorage,
        GOToStorage, 
        TakeResources,
        GOONTaskPosition,
        DeliverResources
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResourceCarrying : Task
    {
        private readonly Resource _resourceToCarry;
        private readonly bool _reservedResources;

        private Vector3 _fromStoragePosition;
        private ResourceCarryingFlag _flag;
        
        private bool IsResourceInDelivery =>
            _flag == ResourceCarryingFlag.DeliverResources ||
            _flag == ResourceCarryingFlag.GOToStorage && worker.Profession.IsCarryingResource;
        
        public Func<ResourceType, int, Resource> resourceWithdraw;
        public Func<Task, int, Resource> reservedResourceWithdraw;
        public Action<Resource> resourceDelivery;

        public Resource ResourceToCarry => _resourceToCarry;

        public ResourceCarrying(Resource resourceToCarry, bool reservedResources, World.Buildings.Building toStorage, World.Buildings.Building fromStorage = null)
        {
            _resourceToCarry = resourceToCarry;
            _reservedResources = reservedResources;

            taskPosition = toStorage.PivotedPosition;

            if (fromStorage != null) {
                _fromStoragePosition = fromStorage.PivotedPosition;
                _flag = ResourceCarryingFlag.GOToStorage;
            }
            else {
                _flag = ResourceCarryingFlag.FindClosestStorage;
            }
        }

        public override void Start()
        {
            worker.Brain.Animations.Turn(_fromStoragePosition);
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
        }
        
        public override void Execute()
        {
            flag = TaskFlag.Running;
            
            switch (_flag) {
                case ResourceCarryingFlag.FindClosestStorage:
                    World.Buildings.Building fromStorage = Managers.I.Buildings
                        .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), taskPosition);
                    _fromStoragePosition = fromStorage.PivotedPosition;

                    if (_reservedResources) 
                        reservedResourceWithdraw += Warehouse.GetReservedResource;
                    else 
                        resourceWithdraw += fromStorage.Storage.WithdrawResource;
                    
                    worker.Brain.Animations.Turn(_fromStoragePosition);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
                    _flag = ResourceCarryingFlag.GOToStorage;
                    break;
                
                case ResourceCarryingFlag.GOToStorage:
                    if (worker.Brain.Motion.MoveTo(_fromStoragePosition))
                        _flag = ResourceCarryingFlag.TakeResources;
                    break;
                
                case ResourceCarryingFlag.TakeResources:
                    int currResAmount = _resourceToCarry.amount;
                    int maxResourceAmount = worker.Profession.Data.ResourceCarryingLimit;
                
                    if (_reservedResources) {
                        worker.Profession.CarriedResource = reservedResourceWithdraw.Invoke(this,
                            currResAmount > maxResourceAmount ? maxResourceAmount : currResAmount);
                    }
                    else {
                        worker.Profession.CarriedResource = resourceWithdraw.Invoke(
                            _resourceToCarry.Type,
                            currResAmount > maxResourceAmount ? maxResourceAmount : currResAmount) ;
                    }
                    
                    worker.UI.SetResourceIcon(_resourceToCarry.Type);
                    _resourceToCarry.amount -= worker.Profession.CarriedResource.amount;
                    _flag = ResourceCarryingFlag.GOONTaskPosition;
                    break;
                
                case ResourceCarryingFlag.GOONTaskPosition:
                    if (worker.Brain.Motion.MoveTo(taskPosition))
                        _flag = ResourceCarryingFlag.DeliverResources;
                    break;
                
                case ResourceCarryingFlag.DeliverResources:
                    resourceDelivery?.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    Debug.LogWarning(worker.name + " has delivered: " + worker.Profession.CarriedResource.amount + " " + worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;

                    if (_resourceToCarry.amount != 0) 
                        _flag = ResourceCarryingFlag.GOToStorage;
                    else 
                        taskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("TASK CARRYING STATE NOT SET");
            }

            worker.UI.StateText.text = "Resource carrying: " + _flag;
        }

        public override void End()
        {
            flag = TaskFlag.Completed;
        }
        public override void Pause() {}

        public override void Abandon()
        {
            if (IsResourceInDelivery) {
                _resourceToCarry.amount += worker.Profession.CarriedResource.amount;
                ThrowResourceOnGround();
            }
            
            _flag = ResourceCarryingFlag.FindClosestStorage;
            base.Abandon();
        }
    }
}
