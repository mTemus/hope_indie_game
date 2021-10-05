using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.Characters.Villagers.Profession;
using HopeMain.Code.System;
using HopeMain.Code.System.Assets;
using HopeMain.Code.World.Buildings.Type.Resources;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.World.Buildings.Workplace
{
    public abstract class Gathering : WorkplaceBase
    {
        protected ResourceType gatheringResourceType;
        protected ResourceToGatherBase gatheringResource;
        
        #region Tasks

        private void RegisterTaskAsExternal(Task task)
        {
            Warehouse warehouse = Managers.I.Buildings
                .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), PivotedPosition) as Warehouse;

            if (warehouse != null) 
                warehouse.RegisterExternalTask(this, task);
            else 
                throw new Exception("CAN'T REGISTER EXTERNAL TASK FROM: " + name.ToUpper() +
                                    " BECAUSE WAREHOUSE NOT EXISTS!");
        }
        
        protected override void AddTaskToDo(Task task)
        {
            if (task is ResourceCarrying && !HasHiredHaulers) {
                RegisterTaskAsExternal(task);
                return;
            }

            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }
            
            foreach (Villager worker in workersWithoutTasks) {
                if (task is ResourceCarrying) {
                    if (worker.Profession.Data.Type != ProfessionType.WorkplaceHauler) continue;
                    GiveTaskToWorker(worker, task);
                    Debug.Log("Added to hauler: " + worker.name);
                    return;
                }
                
                if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) continue;
                GiveTaskToWorker(worker, task);
                Debug.Log("Added to: " + worker.name);
                return;
            }
            
            tasksToDo.Add(task);
            Debug.Log("Added task as todo.");
        }

        //TODO: stopping/recovering tasks should get an update when worker AI will be updated
        protected void StopAllTasks()
        {
            foreach (var worker in workers
                .Where(worker => worker.Profession.Data.Type != ProfessionType.WorkplaceHauler)) 
            { worker.Brain.Work.CompleteTask(); }
            
            
            Storage.onResourceLimitReset.AddListener(RecoverAllTasks);
            Storage.onResourceLimitReset.AddListener(() => Storage.onResourceLimitReset.RemoveAllListeners());
        }

        private void RecoverAllTasks()
        {
            int tasksNeeded = workersWithoutTasks.Count(worker => worker.Profession.Data.Type != ProfessionType.WorkplaceHauler);
            
            //TODO: event or sth
            // for (int i = 0; i < tasksNeeded; i++) 
            //     CreateResourceGatheringTask();
        }
        
        #endregion
        
        #region Resources

        protected virtual void CreateSingleResourceGatheringTask()
        {
            Resources.ResourceToGather.Data rtgd = AssetsStorage.I.GetResourceToGatherDataByResourceType(gatheringResourceType);
            ResourceGathering rgt = new ResourceGathering_Single(rtgd.ResourceType, rtgd.OccurAreas);
            rgt.onResourceDelivery += Storage.StoreResource;
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }
        
        protected virtual void CreateSpotResourceGatheringTask()
        {
            ResourceGathering rgt = new ResourceGathering_Spot(gatheringResource);
            rgt.onResourceDelivery += Storage.StoreResource;
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }
        
        public virtual void DeliverStoredResources(Resource storedResource)
        {
            if (!(Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse warehouse)) return;

            ResourceCarrying rct;
            
            if (HasHiredHaulers) {
                rct = new ResourceCarrying(storedResource, false, warehouse, this);
                rct.onResourceDelivery += warehouse.StoreResource;
                rct.onResourceWithdraw += Storage.WithdrawResourceContinuously;
                AddTaskToDo(rct);
            }
            else if (storedResource.amount >= AssetsStorage.I.GetProfessionDataForProfessionType(ProfessionType.GlobalHauler).ResourceCarryingLimit) {
                rct = new ResourceCarrying(storedResource, false, warehouse, this);
                rct.onResourceDelivery += warehouse.StoreResource;
                rct.onResourceWithdraw += Storage.WithdrawResource;
                AddTaskToDo(rct);
            }
        }
        
        public void TakeTasksBackFromWarehouse()
        {
            List<Warehouse> warehouses = Managers.I.Buildings
                .GetAllBuildingOfClass(BuildingType.Resources, typeof(Warehouse)).Cast<Warehouse>().ToList();

            List<Task> externalTasks = new List<Task>();

            foreach (Warehouse warehouse in warehouses
                .Where(warehouse => warehouse.HasExternalTasksFromWorkplace(this))) 
                externalTasks.AddRange(warehouse.GetExternalTasksBackToWorkplace(this));
            
            foreach (Task externalTask in externalTasks) 
                AddTaskToDo(externalTask);
        }
        
        #endregion
    }
}
