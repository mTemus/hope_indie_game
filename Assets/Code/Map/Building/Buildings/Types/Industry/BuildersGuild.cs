using System.Linq;
using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building.Buildings.Types.Industry
{
    public class BuildersGuild : ServicesWorkplace
    {
        public override void Initialize() {}
        
        #region Tasks

        protected override Task GetNormalTask()
        {
            Task nt = (from task in tasksToDo
                    where task is Task_Building 
                    select task as Task_Building)
                .FirstOrDefault(btt => btt.ResourcesDelivered);

            RemoveTaskFromTodoList(nt);
            return nt;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is Task_ResourceCarrying
                    select task as Task_ResourceCarrying)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }
        
        private void DeliverResourcesToConstruction(Resource requiredResource, Construction construction)
        {
            Task_ResourceCarrying rct =
                new Task_ResourceCarrying(requiredResource, true, construction.GetComponent<Building>());
            rct.onResourceDelivery += construction.AddResources;
            
            Managers.I.Resources.ReserveResources(rct, requiredResource);
            AddTaskToDo(rct);
        }

        protected override void AddTaskToDo(Task task)
        {
            Debug.Log("Adding task as to do: " + task.GetType().Name);

            if (task is Task_Building {ResourcesDelivered: false}) {
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
                if (task is Task_ResourceCarrying) {
                    if (haulersCnt > 0) {
                        if (worker.Profession.Data.Type != ProfessionType.WorkplaceHauler) continue;
                        GiveTaskToWorker(worker, task);
                        Debug.Log("Added to hauler: " + worker.name);
                        return;
                    }
                    
                    GiveTaskToWorker(worker, task);
                    Debug.Log("Added to: " + worker.name);
                    return;
                }
                
                if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                    continue;
                    
                GiveTaskToWorker(worker, task);
                Debug.Log("Added to: " + worker.name);
                return;
            }
            
            tasksToDo.Add(task);
            Debug.Log("Added task as todo.");
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            if (task is Task_ResourceCarrying rct) {
                if (Managers.I.Resources.IsResourceReserved(rct)) {
                    if (!Managers.I.Resources.CanWithdrawReserved(rct, rct.ResourceToCarry)) 
                        Managers.I.Resources.ClearReservedResource(rct);
                    else {
                        AddTaskToDo(rct);
                        return;
                    }
                }

                // If task still exist but resources are not reserved it's always mean that it was abandoned
                // while carrying resource state, so it need to have resource reserved again
                
                if (Managers.I.Resources.CanReserveResource(rct.ResourceToCarry)) {
                    Managers.I.Resources.ReserveResources(rct, rct.ResourceToCarry);
                    AddTaskToDo(rct);
                }
                else {
                    Managers.I.Resources.AddWaitingTask(rct, rct.ResourceToCarry);
                    rct.onTaskSetReady += SetTaskReady;
                    SetTaskWaiting(rct);
                }
            }
            else {
                AddTaskToDo(task);
            }
        }

        public void CreateBuildingTask(Construction construction, BuildingData buildingData)
        {
            Task_Building bt = new Task_Building(construction.transform.position + construction.PositionOffset, construction);
            bt.onTaskSetReady += SetTaskReady;
            construction.SetBuildingTask(bt);
            AddTaskToDo(bt);

            foreach (Resource resource in buildingData.RequiredResources) 
                DeliverResourcesToConstruction(new Resource(resource), construction);
        }

        #endregion

        #region Workers

        protected override void FireNormalWorker(Villager worker)
        {
            Debug.Log("Fired worker: " + worker.name + " from: " + name);
        }

        #endregion
    }
}
