using System;
using System.Collections.Generic;
using System.Linq;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class Sawmill : Workplace
    {
        protected override Task GetNormalTask() 
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceGatheringTask
                    select task as ResourceGatheringTask)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceCarryingTask
                    select task as ResourceCarryingTask)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override void AddTaskToDo(Task task)
        {
            Debug.Log("Adding task as to do: " + task.GetType().Name);

            if (task is ResourceCarryingTask rct) {
                if (!HasHiredHaulers) {
                    Warehouse warehouse = Managers.I.Buildings
                        .GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse), PivotedPosition) as Warehouse;

                    if (warehouse != null) 
                        warehouse.RegisterExternalTask(this, rct);
                    else 
                        throw new Exception("CAN'T REGISTER EXTERNAL TASK FROM: " + name.ToUpper() +
                                            " BECAUSE WAREHOUSE NOT EXISTS!");
                    return;
                }
            }
            
            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }
            
            foreach (Villager worker in workersWithoutTasks) {
                if (task is ResourceCarryingTask) {
                    if (HasHiredHaulers) {
                        if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) {
                            GiveTaskToWorker(worker, task);
                            Debug.Log("Added to hauler: " + worker.name);
                            return;
                        }
                    }
                    else {
                        GiveTaskToWorker(worker, task);
                        Debug.Log("Added to: " + worker.name);
                        return;
                    }
                }
                else {
                    if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                        continue;
                    
                    GiveTaskToWorker(worker, task);
                    Debug.Log("Added to: " + worker.name);
                    return;
                }
            }
            
            tasksToDo.Add(task);
            Debug.Log("Added task as todo.");
        }

        public override void SetAutomatedTask()
        {
            CreateResourceGatheringTask();
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            AddTaskToDo(task);
        }

        protected override void FireNormalWorker(Villager worker)
        {
            throw new NotImplementedException();
        }

        public override void DeliverStoredResources(Resource storedResource)
        {
            // if (!workersWithoutTasks.Any(worker => worker.Profession is VillagerWorkplaceHauler)) return;
            if (!(Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse w)) return;

            ResourceCarryingTask rct;
            
            if (HasHiredHaulers) {
                rct = new ResourceCarryingTask(storedResource, w,
                    w.StoreResource, Storage.WithdrawResourceContinuously, this);
                AddTaskToDo(rct);
            }
            else {
                if (storedResource.amount < AssetsStorage.I
                    .GetProfessionDataForProfessionType(ProfessionType.GlobalHauler).ResourceCarryingLimit) return;
                rct = new ResourceCarryingTask(new Resource(storedResource), w,
                    w.StoreResource, Storage.WithdrawResource, this);
                AddTaskToDo(rct);
            }
        }

        private void CreateResourceGatheringTask()
        {
            if (IsGatheringTaskTodo()) return;
            ResourceToGatherData rtgd = AssetsStorage.I.GetResourceToGatherDataByResourceType(ResourceType.WOOD);
            ResourceGatheringTask rgt = new ResourceGatheringTask(Storage, rtgd.ResourceType, rtgd.OccurAreas);
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }

        public void TakeTasksBackFromWarehouse()
        {
            List<Warehouse> warehouses = Managers.I.Buildings
                .GetAllBuildingOfClass(BuildingType.Resources, typeof(Warehouse)).Cast<Warehouse>().ToList();

            List<Task> externalTasks = new List<Task>();

            foreach (Warehouse warehouse in warehouses.Where(warehouse => warehouse.HasExternalTasksFromWorkplace(this))) 
                externalTasks.AddRange(warehouse.GetExternalTasksBackToWorkplace(this));
            
            foreach (Task externalTask in externalTasks) 
                AddTaskToDo(externalTask);
        }

        private bool IsGatheringTaskTodo() =>
            tasksToDo.Any(task => task is ResourceGatheringTask);
    }
}
