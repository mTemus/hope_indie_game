using System;
using System.Collections.Generic;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.World.Buildings.Type.Resources;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResourcePickUpFlag 
    {
        GOToStorage,
        GETResourcesData,
        GOToResource,
        CollectResource,
        DeliverResource
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class ResourcePickUp : Task
    {
        private Queue<ResourceToPickUp> _resources = new Queue<ResourceToPickUp>();

        private ResourcePickUpFlag _flag;
        private ResourceToPickUp _currentResource;
        private Warehouse _warehouse;

        private int _resourceAmount;

        public ResourceType StoredResourceType { get; }
        public bool HasWorker => worker != null;
        public bool CanStoreResources => _resourceAmount < worker.Profession.Data.ResourceCarryingLimit;
        public bool IsResourceInDelivery =>
            _flag == ResourcePickUpFlag.DeliverResource || 
            _flag == ResourcePickUpFlag.GOToStorage && worker.Profession.IsCarryingResource;
        
        public ResourcePickUp(ResourceType storedResourceType)
        {
            StoredResourceType = storedResourceType;
        }
        
        private void SortResources()
        {
            Vector3 workerPosition = worker.transform.position;
            List<ResourceToPickUp> resourcesList = new List<ResourceToPickUp>(_resources);

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

            _resources = new Queue<ResourceToPickUp>(resourcesList);
        }

        private void GetNextResource()
        {
            if (_resources.Count > 0) {
                _currentResource = _resources.Dequeue();
                _flag = ResourcePickUpFlag.GOToResource;
            }
            else {
                if (worker.Profession.IsCarryingResource) 
                    _flag = ResourcePickUpFlag.GOToStorage;
                else 
                    taskCompleted.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool AddResourceToPickUp(ResourceToPickUp resource)
        {
            int newAmount = _resourceAmount + resource.StoredResource.amount;
            if (newAmount > worker.Profession.Data.ResourceCarryingLimit)
                return false;
            
            _resourceAmount = newAmount;
            _resources.Enqueue(resource);
            SortResources();
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        public void RemoveResourceBeforePickUp(ResourceToPickUp resource)
        {
            if (_currentResource == resource) 
                GetNextResource();
            
            _resourceAmount -= resource.StoredResource.amount;
            List<ResourceToPickUp> resourcesList = new List<ResourceToPickUp>(_resources);
            resourcesList.Remove(resource);
            _resources = new Queue<ResourceToPickUp>(resourcesList);
        }

        public override void Start()
        {
            switch (flag) {
                case TaskFlag.New:
                    _warehouse = worker.Profession.Workplace as Warehouse;
                    
                    if (_warehouse == null) 
                        throw new Exception("CAN'T GET WAREHOUSE REFERENCE FROM WORKER " + worker.name + " IN TASK " + GetType());

                    _flag = ResourcePickUpFlag.GETResourcesData;
                    break;
                
                case TaskFlag.Waiting:
                    break;
                case TaskFlag.Running:
                    break;
                case TaskFlag.Interrupted:
                    break;
                case TaskFlag.Paused:
                    break;
                case TaskFlag.Abandoned:
                    break;
                case TaskFlag.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
        }

        public override void Execute()
        {
            flag = TaskFlag.Running;
            
            switch (_flag) {
                case ResourcePickUpFlag.GOToStorage:
                    if (worker.Brain.Motion.MoveTo(_warehouse.PivotedPosition)) 
                        _flag = worker.Profession.CarriedResource != null ? 
                            ResourcePickUpFlag.DeliverResource : ResourcePickUpFlag.GETResourcesData;
                    break;
                
                case ResourcePickUpFlag.GETResourcesData:
                    _warehouse.GetResourcesToPickUpByType(this);
                    GetNextResource();
                    _flag = ResourcePickUpFlag.GOToResource;
                    break;
                
                case ResourcePickUpFlag.GOToResource:
                    if (worker.Brain.Motion.MoveTo(_currentResource.transform.position))
                        _flag = ResourcePickUpFlag.CollectResource;
                    break;
                
                case ResourcePickUpFlag.CollectResource:
                    worker.UI.SetResourceIcon(_currentResource.StoredResource.Type);
                    Resource collectedResource = _currentResource.WithdrawResource();
                    Resource workerResource = worker.Profession.CarriedResource;

                    if (workerResource == null) 
                        worker.Profession.CarriedResource = collectedResource;
                    else 
                        workerResource.amount += collectedResource.amount;
                    
                    _warehouse.UnregisterResourceToPickUp(_currentResource);
                    GetNextResource();
                    break;
                
                case ResourcePickUpFlag.DeliverResource:
                    _warehouse.StoreResource(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    worker.Profession.CarriedResource = null;
                    taskCompleted.Invoke();
                    break;
                
                default:
                    throw new Exception("NO SUCH STATE FOR RESOURCE PICK UP TASK");
            }
        }
        
        public override void End() {}
        public override void Pause() {}
    }
}
