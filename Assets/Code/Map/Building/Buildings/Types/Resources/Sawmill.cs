using System;
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

        private void CreateResourceGatheringTask()
        {
            if (IsGatheringTaskTodo()) return;
            ResourceToGatherData rtgd = AssetsStorage.I.GetResourceToGatherDataByResourceType(ResourceType.WOOD);
            ResourceGatheringTask rgt = new ResourceGatheringTask(GetComponent<Building>().Storage, rtgd.ResourceType, rtgd.OccurAreas);
            AddTaskToDo(rgt);
            
            Debug.LogError("Created automated task in: " + name);
        }

        private bool IsGatheringTaskTodo() =>
            tasksToDo.Any(task => task is ResourceGatheringTask);
    }
}
