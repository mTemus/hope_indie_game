using System;
using System.Collections.Generic;
using System.Linq;
using Code.Map.Building.Buildings.Types.Resources;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building.Workplaces
{
    public abstract class GatheringWorkplace : Workplace
    {
        protected ResourceType gatheringResourceType;
        
        private bool IsGatheringTaskTodo =>
            tasksToDo.Any(task => task is ResourceGatheringTask);
        
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
            if (task is ResourceCarryingTask && !HasHiredHaulers) {
                RegisterTaskAsExternal(task);
                return;
            }

            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }
            
            foreach (Villager worker in workersWithoutTasks) {
                if (task is ResourceCarryingTask) {
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
        
        #endregion
        
        #region Resources

        protected virtual void CreateResourceGatheringTask()
        {
            if (IsGatheringTaskTodo) return;
            ResourceToGatherData rtgd = AssetsStorage.I.GetResourceToGatherDataByResourceType(gatheringResourceType);
            ResourceGatheringTask rgt = new ResourceGatheringTask(rtgd.ResourceType, rtgd.OccurAreas);
            rgt.onResourceDelivery += Storage.StoreResource;
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }
        
        public virtual void DeliverStoredResources(Resource storedResource)
        {
            if (!(Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse warehouse)) return;

            ResourceCarryingTask rct;
            
            if (HasHiredHaulers) {
                rct = new ResourceCarryingTask(storedResource, false, warehouse, this);
                rct.onResourceDelivery += warehouse.StoreResource;
                rct.onResourceWithdraw += Storage.WithdrawResourceContinuously;
                AddTaskToDo(rct);
            }
            else if (storedResource.amount >= AssetsStorage.I.GetProfessionDataForProfessionType(ProfessionType.GlobalHauler).ResourceCarryingLimit) {
                rct = new ResourceCarryingTask(storedResource, false, warehouse, this);
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