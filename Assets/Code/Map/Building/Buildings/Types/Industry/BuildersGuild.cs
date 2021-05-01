using System.Linq;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Code.Map.Building.Buildings.Types.Industry
{
    public class BuildersGuild : Workplace
    {
        #region Tasks

        protected override Task GetNormalTask()
        {
            Task nt = (from task in tasksToDo
                    where task is BuildingTask 
                    select task as BuildingTask)
                .FirstOrDefault(btt => btt.ResourcesDelivered);

            RemoveTaskFromTodoList(nt);
            return nt;
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
        
        private void CreateResourceCarryingTask(Resource requiredResource, Construction construction)
        {
            ResourceCarryingTask rct = new ResourceCarryingTask(requiredResource, construction.GetComponent<Building>(),
                construction.AddResources, true);
            
            Managers.Instance.Resources.ReserveResources(rct, requiredResource);
            AddTaskToDo(rct);
        }

        protected override void AddTaskToDo(Task task)
        {
            Debug.Log("Adding task as to do: " + task.GetType().Name);

            if (task is BuildingTask {ResourcesDelivered: false}) {
                Debug.Log("Added as waiting.");
                SetTaskWaiting(task);
                return;
            }

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
            // No automated task at builders guild, at least yet.
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            if (task is ResourceCarryingTask rct) {
                if (Managers.Instance.Resources.IsResourceReserved(rct)) {
                    if (!Managers.Instance.Resources.CanWithdrawReserved(rct, rct.CarriedResource)) 
                        Managers.Instance.Resources.ClearReservedResource(rct);
                    else {
                        AddTaskToDo(rct);
                        return;
                    }
                }

                // If task still exist but resources are not reserved it's always mean that it was abandoned
                // while carrying resource state, so it need to have resource reserved again
                
                if (Managers.Instance.Resources.CanReserveResource(rct.CarriedResource)) {
                    Managers.Instance.Resources.ReserveResources(rct, rct.CarriedResource);
                    AddTaskToDo(rct);
                }
                else {
                    Managers.Instance.Resources.AddWaitingTask(rct, rct.CarriedResource);
                    rct.onTaskSetReady += SetTaskReady;
                    SetTaskWaiting(rct);
                }
            }
            else {
                AddTaskToDo(task);
            }
        }

        protected override void FireNormalWorker(Villager worker)
        {
            Debug.Log("Fired worker: " + worker.name + " from: " + name);
        }

        public override void DeliverStoredResources(Resource storedResource)
        {
            throw new NotImplementedException();
        }

        public void CreateBuildingTask(Construction construction, BuildingData buildingData)
        {
            BuildingTask bt = new BuildingTask(0, construction.transform.position + construction.PositionOffset, construction);
            bt.onTaskSetReady += SetTaskReady;
            construction.SetBuildingTask(bt);
            AddTaskToDo(bt);

            foreach (Resource resource in buildingData.RequiredResources) 
                CreateResourceCarryingTask(new Resource(resource), construction);
        }

        #endregion
    }
}
