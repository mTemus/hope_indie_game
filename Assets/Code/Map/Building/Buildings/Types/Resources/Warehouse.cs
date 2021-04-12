using System.Collections.Generic;
using System.Linq;
using Code.Resources;
using Code.System;
using NotImplementedException = System.NotImplementedException;
using Task = Code.Villagers.Tasks.Task;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class Warehouse : Workplace
    {
        private readonly List<Resource> storedResources = new List<Resource>();

        private void Start()
        {
            StoreResource(new Resource(ResourceType.WOOD, 300));
            StoreResource(new Resource(ResourceType.STONE, 300));
        }
        
        private Resource GetResource(ResourceType resource) =>
            storedResources.FirstOrDefault(res => res.Type == resource);
        
        public void StoreResource(Resource resourceToStore)
        {
            Resource storedResource = GetResource(resourceToStore.Type);

            if (storedResource == null) 
                storedResources.Add(resourceToStore);
            else 
                storedResource.amount += resourceToStore.amount;
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
            throw new NotImplementedException();
        }

        protected override Task GetResourceCarryingTask()
        {
            throw new NotImplementedException();
        }

        protected override void AddTaskToDo(Task task)
        {
            throw new NotImplementedException();
        }

        public override void SetAutomatedTask()
        {
            throw new NotImplementedException();
        }
    }
}
