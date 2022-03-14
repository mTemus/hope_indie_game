using System.Linq;
using _Prototype.Code.v001.AI.Villagers.Tasks;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.Characters.Villagers.Professions;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.World.Buildings.Workplaces;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings.Type.Industry
{
    /// <summary>
    /// 
    /// </summary>
    public class BuildersGuild : Services
    {
        public override void Initialize() {}
        
        #region Tasks

        protected override Task GetNormalTask()
        {
            Task nt = (from task in tasksToDo
                    where task is AI.Villagers.Tasks.Building
                    select task as AI.Villagers.Tasks.Building)
                .FirstOrDefault(bt => bt.flag != TaskFlag.Waiting);

            RemoveTaskFromTodoList(nt);
            return nt;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceCarrying
                    select task as ResourceCarrying)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }
        
        private void DeliverResourcesToConstruction(Resource requiredResource, Construction construction)
        {
            ResourceCarrying rct =
                new ResourceCarrying(requiredResource, true, construction.GetComponent<Building>());
            rct.resourceDelivery += construction.AddResources;
            
            Managers.I.Resources.ReserveResources(rct, requiredResource);
            AddTaskToDo(rct);
        }

        protected override void AddTaskToDo(Task task)
        {
            Debug.Log("Adding task as to do: " + task.GetType().Name);

            if (task.flag == TaskFlag.Waiting) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo, because it's state is \"WAITING\".");
                return;
            }

            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }
            
            foreach (Villager worker in workersWithoutTasks) {
                if (task is ResourceCarrying) {
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
            if (task is ResourceCarrying rct) {
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
                    rct.taskSetReady += rct.SetReady;
                    rct.flag = TaskFlag.Waiting;
                }
            }
            else {
                AddTaskToDo(task);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="construction"></param>
        /// <param name="buildingData"></param>
        public void CreateBuildingTask(Construction construction, Data buildingData)
        {
            AI.Villagers.Tasks.Building bt = new AI.Villagers.Tasks.Building(construction.transform.position + construction.PositionOffset, construction);
            bt.SetWaiting();
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
