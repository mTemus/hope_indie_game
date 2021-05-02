using System;
using System.Linq;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Professions.Types;
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

            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }
            
            foreach (Villager worker in workersWithoutTasks) {
                if (task is ResourceCarryingTask) {
                    if (haulersCnt > 0) {
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
            if (!workersWithoutTasks.Any(worker => worker.Profession is VillagerWorkplaceHauler)) return;
            if (!(Managers.Instance.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse w)) return;
            
            ResourceCarryingTask rct = new ResourceCarryingTask(storedResource, w,
                w.StoreResource, Storage.WithdrawResourceContinuously, this);
            
            AddTaskToDo(rct);
        }

        private void CreateResourceGatheringTask()
        {
            if (IsGatheringTaskTodo()) return;
            ResourceToGatherData rtgd = AssetsStorage.I.GetResourceToGatherDataByResourceType(ResourceType.WOOD);
            ResourceGatheringTask rgt = new ResourceGatheringTask(Storage, rtgd.ResourceType, rtgd.OccurAreas);
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }

        private bool IsGatheringTaskTodo() =>
            tasksToDo.Any(task => task is ResourceGatheringTask);
    }
}
