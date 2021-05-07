using System.Collections.Generic;
using System.Linq;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;
using UnityEngine;
using Task = Code.Villagers.Tasks.Task;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class Warehouse : Workplace
    {
        private readonly List<Resource> storedResources = new List<Resource>();
        private readonly List<ResourceToPickUp> resourcesToPickUp = new List<ResourceToPickUp>();
        private readonly Dictionary<Workplace, List<Task>> externalTasks = new Dictionary<Workplace, List<Task>>();
        
        private void Start()
        {
            StoreResource(new Resource(ResourceType.WOOD, 300));
            StoreResource(new Resource(ResourceType.STONE, 300));
        }

        #region ResourcesToPickUp

        public void RegisterResourceToPickUp(ResourceToPickUp resource)
        {
            ResourcePickUpTask rtpt = null;
            
            foreach (Task task in tasksToDo) {
                if (!(task is ResourcePickUpTask {HasWorker: true} rpt)) continue;
                if (rpt.IsResourceInDelivery) continue;
                if (rpt.CanStoreResources) {
                    if (rpt.AddResourceToPickUp(resource))
                        resource.ResourcePickUpTask = rpt;
                }
                else {
                    rtpt = new ResourcePickUpTask(resource.StoredResource.Type);
                    AddTaskToDo(rtpt); 
                }
            }

            if (rtpt == null) {
                rtpt = new ResourcePickUpTask(resource.StoredResource.Type);
                AddTaskToDo(rtpt); 
            }
            
            resourcesToPickUp.Add(resource);
        }
        
        public void UnregisterResourceToPickUp(ResourceToPickUp resource)
        {
            resourcesToPickUp.Remove(resource);
        }

        public void GetResourcesToPickUp(ResourcePickUpTask rput)
        {
            foreach (var resource in resourcesToPickUp
                .Where(resource => resource.StoredResource.Type == rput.StoredResourceType)
                .Where(resource => !resource.IsRegisteredToPickUp)) {
                if (rput.AddResourceToPickUp(resource))
                    resource.ResourcePickUpTask = rput;
                if (!rput.CanStoreResources) break;
            }
        }

        #endregion

        #region NormalResources

        private Resource GetResource(ResourceType resource) =>
            storedResources.FirstOrDefault(res => res.Type == resource);
        
        public void StoreResource(Resource resourceToStore)
        {
            Resource storedResource = GetResource(resourceToStore.Type);

            if (storedResource == null) 
                storedResources.Add(resourceToStore);
            else 
                storedResource.amount += resourceToStore.amount;
            
            Debug.LogWarning("Stored: " + resourceToStore.amount + " " + resourceToStore.Type);
        }

        #endregion

        #region ReservedResources

        public static Resource GetReservedResource(Task tKey, int amount)
        {
            Resource reservedResource = Managers.I.Resources.WithdrawReservedResource(tKey);
            Resource takenResource = new Resource(reservedResource.Type);
            
            reservedResource.amount -= amount;
            takenResource.amount = amount;

            if (reservedResource.amount == 0) 
                Managers.I.Resources.ClearReservedResource(tKey);
            
            return takenResource;
        }

        #endregion

        #region ExternalTasks

        private Task GetExternalTask()
        {
            List<Workplace> keys = externalTasks.Keys.ToList();
            Task et = externalTasks[keys[0]][0];
            externalTasks[keys[0]].Remove(et);

            if (externalTasks[keys[0]].Count == 0) 
                externalTasks.Remove(keys[0]);

            return et;
        }
        
        public bool HasExternalTasksFromWorkplace(Workplace workplace) =>
            externalTasks.ContainsKey(workplace);
        
        public void RegisterExternalTask(Workplace workplace, Task task)
        {
            if (workersWithoutTasks.Count > 0) {
                GiveTaskToWorker(workersWithoutTasks[0], task);
                return;
            }
            
            if (externalTasks.ContainsKey(workplace)) 
                externalTasks[workplace].Add(task);
            else 
                externalTasks[workplace] = new List<Task> { task };
        }

        public List<Task> GetExternalTasksBackToWorkplace(Workplace workplace)
        {
            List<Task> tasks = new List<Task>(externalTasks[workplace]);
            externalTasks.Remove(workplace);
            return tasks;
        }

        #endregion
        
        #region Tasks

        protected override Task GetNormalTask()
        {
            Task nt = tasksToDo[0];
            RemoveTaskFromTodoList(nt);
            return nt;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = GetExternalTask();
            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override void AddTaskToDo(Task task)
        {
            if (workersWithoutTasks.Count == 0) {
                tasksToDo.Add(task);
                Debug.Log("Added task as todo.");
                return;
            }

            Villager worker = workersWithoutTasks[0];
            GiveTaskToWorker(worker, task);
        }

        public override void SetAutomatedTask()
        {
            
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            AddTaskToDo(task);
        }

        #endregion

        #region Workers

        protected override void FireNormalWorker(Villager worker)
        {
        }

        public override void DeliverStoredResources(Resource storedResource)
        {
        }

        #endregion
        
    }
}
