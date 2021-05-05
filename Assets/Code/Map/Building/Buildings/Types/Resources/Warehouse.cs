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
        
        private void Start()
        {
            StoreResource(new Resource(ResourceType.WOOD, 300));
            StoreResource(new Resource(ResourceType.STONE, 300));
        }
        
        private Resource GetResource(ResourceType resource) =>
            storedResources.FirstOrDefault(res => res.Type == resource);

        public void RegisterResourceToPickUp(ResourceToPickUp resource)
        {
            ResourcePickUpTask rtpt = null;
            
            foreach (Task task in tasksToDo) {
                if (!(task is ResourcePickUpTask rpt)) continue;
                if (!rpt.HasWorker) continue;
                if (rpt.IsResourceInDelivery) continue;
                if (rpt.CanStoreResources) {
                    if (rpt.AddResourceToPickUp(resource)) 
                        resource.OnResourceRegisterToPickUp(rpt);
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
                    resource.OnResourceRegisterToPickUp(rput);
                if (!rput.CanStoreResources) break;
            }
        }

        public void StoreResource(Resource resourceToStore)
        {
            Resource storedResource = GetResource(resourceToStore.Type);

            if (storedResource == null) 
                storedResources.Add(resourceToStore);
            else 
                storedResource.amount += resourceToStore.amount;
            
            Debug.LogWarning("Stored: " + resourceToStore.amount + " " + resourceToStore.Type);
        }
        
        public Resource GetReservedResource(Task tKey, int amount)
        {
            Resource reservedResource = Managers.Instance.Resources.WithdrawReservedResource(tKey);
            Resource takenResource = new Resource(reservedResource.Type);
            
            reservedResource.amount -= amount;
            takenResource.amount = amount;

            if (reservedResource.amount == 0) 
                Managers.Instance.Resources.ClearReservedResource(tKey);
            
            return takenResource;
        }

        protected override Task GetNormalTask()
        {
            Task nt = tasksToDo.FirstOrDefault(task => !(task is ResourceCarryingTask));
            RemoveTaskFromTodoList(nt);
            return nt;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = tasksToDo.FirstOrDefault(task => task is ResourceCarryingTask);
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

        protected override void FireNormalWorker(Villager worker)
        {
        }

        public override void DeliverStoredResources(Resource storedResource)
        {
        }
    }
}
