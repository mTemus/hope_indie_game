using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.AI.Villagers.Tasks;
using _Prototype.Code.Characters.Villagers.Entity;
using _Prototype.Code.System;
using _Prototype.Code.World.Resources;
using UnityEngine;

namespace _Prototype.Code.World.Buildings.Type.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public class Warehouse : Workplaces.Workplace
    {
        private readonly List<Resource> _storedResources = new List<Resource>();
        private readonly List<ResourceToPickUp> _resourcesToPickUp = new List<ResourceToPickUp>();
        private readonly Dictionary<Workplaces.Workplace, List<Task>> _externalTasks = new Dictionary<Workplaces.Workplace, List<Task>>();

        public override void Initialize()
        {
            StoreResource(new Resource(ResourceType.Wood, 300));
            StoreResource(new Resource(ResourceType.Stone, 300));
        }

        #region ResourcesToPickUp

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
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
            
            _resourcesToPickUp.Add(resource);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        public void UnregisterResourceToPickUp(ResourceToPickUp resource)
        {
            _resourcesToPickUp.Remove(resource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rput"></param>
        public void GetResourcesToPickUpByType(ResourcePickUp rput)
        {
            foreach (var resource in _resourcesToPickUp
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
            _storedResources.FirstOrDefault(res => res.Type == resource);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceToStore"></param>
        public void StoreResource(Resource resourceToStore)
        {
            Resource storedResource = GetResource(resourceToStore.Type);

            if (storedResource == null) 
                _storedResources.Add(resourceToStore);
            else 
                storedResource.amount += resourceToStore.amount;
            
            Debug.LogWarning("Stored: " + resourceToStore.amount + " " + resourceToStore.Type);
        }

        #endregion

        #region ReservedResources

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
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
            List<Workplaces.Workplace> keys = _externalTasks.Keys.ToList();
            Task et = _externalTasks[keys[0]][0];
            _externalTasks[keys[0]].Remove(et);

            if (_externalTasks[keys[0]].Count == 0) 
                _externalTasks.Remove(keys[0]);

            return et;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workplace"></param>
        /// <returns></returns>
        public bool HasExternalTasksFromWorkplace(Workplaces.Workplace workplace) =>
            _externalTasks.ContainsKey(workplace);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workplace"></param>
        /// <param name="task"></param>
        public void RegisterExternalTask(Workplaces.Workplace workplace, Task task)
        {
            if (workersWithoutTasks.Count > 0) {
                GiveTaskToWorker(workersWithoutTasks[0], task);
                return;
            }
            
            if (_externalTasks.ContainsKey(workplace)) 
                _externalTasks[workplace].Add(task);
            else 
                _externalTasks[workplace] = new List<Task> { task };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workplace"></param>
        /// <returns></returns>
        public List<Task> GetExternalTasksBackToWorkplace(Workplaces.Workplace workplace)
        {
            List<Task> tasks = new List<Task>(_externalTasks[workplace]);
            _externalTasks.Remove(workplace);
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
