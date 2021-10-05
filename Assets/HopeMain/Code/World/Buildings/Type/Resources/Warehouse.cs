using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.World.Buildings.Type.Resources
{
    public class Warehouse : WorkplaceBase
    {
        private readonly List<Resource> storedResources = new List<Resource>();
        private readonly List<ResourceToPickUp> resourcesToPickUp = new List<ResourceToPickUp>();
        private readonly Dictionary<WorkplaceBase, List<Task>> externalTasks = new Dictionary<WorkplaceBase, List<Task>>();

        public override void Initialize()
        {
            StoreResource(new Resource(ResourceType.WOOD, 300));
            StoreResource(new Resource(ResourceType.STONE, 300));
        }

        #region ResourcesToPickUp

        public void RegisterResourceToPickUp(ResourceToPickUp resource)
        {
            ResourcePickUp rtpt = null;
            
            foreach (Task task in tasksToDo) {
                if (!(task is ResourcePickUp {HasWorker: true} rpt)) continue;
                if (rpt.IsResourceInDelivery) continue;
                if (rpt.CanStoreResources) {
                    if (rpt.AddResourceToPickUp(resource))
                        resource.ResourcePickUpTask = rpt;
                }
                else {
                    rtpt = new ResourcePickUp(resource.StoredResource.Type);
                    AddTaskToDo(rtpt); 
                }
            }

            if (rtpt == null) {
                rtpt = new ResourcePickUp(resource.StoredResource.Type);
                AddTaskToDo(rtpt); 
            }
            
            resourcesToPickUp.Add(resource);
        }
        
        public void UnregisterResourceToPickUp(ResourceToPickUp resource)
        {
            resourcesToPickUp.Remove(resource);
        }

        public void GetResourcesToPickUpByType(ResourcePickUp rput)
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
            List<WorkplaceBase> keys = externalTasks.Keys.ToList();
            Task et = externalTasks[keys[0]][0];
            externalTasks[keys[0]].Remove(et);

            if (externalTasks[keys[0]].Count == 0) 
                externalTasks.Remove(keys[0]);

            return et;
        }
        
        public bool HasExternalTasksFromWorkplace(WorkplaceBase workplace) =>
            externalTasks.ContainsKey(workplace);
        
        public void RegisterExternalTask(WorkplaceBase workplace, Task task)
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

        public List<Task> GetExternalTasksBackToWorkplace(WorkplaceBase workplace)
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

        public override void TakeTaskBackFromWorker(Task task)
        {
            AddTaskToDo(task);
        }

        #endregion

        #region Workers

        protected override void FireNormalWorker(Villager worker)
        {
        }

        #endregion
    }
}
