using System.Collections.Generic;
using System.Linq;
using Code.Resources;
using UnityEngine;

namespace Code.Map.Building.Buildings.Components
{
    public class Warehouse : Building
    {
        private readonly List<Resource> storedResources = new List<Resource>();

        private void Start()
        {
            StoreResource(new Resource(ResourceType.WOOD, 300));
            StoreResource(new Resource(ResourceType.STONE, 300));
        }
        
        private bool CanWithdraw(Resource storedResource, int amount)
        {
            if (storedResource == null) return false;
            if (storedResource.amount - amount <= 0) return false;

            return true;
        }
        
        private Resource GetResource(ResourceType resource) =>
            storedResources.FirstOrDefault(res => res.Type == resource);

        public void StoreResources(Resource resource)
        {
            Resource storedResource = GetResource(resource.Type);

            if (storedResource != null) 
                storedResource.amount += resource.amount;
            else 
                storedResources.Add(resource);
        }
        
        public Resource GetResource(ResourceType resource, int amount)
        {
            Resource storedResource = GetResource(resource);
            Resource takenResource = new Resource(resource);

            if (!CanWithdraw(storedResource, amount)) {
                takenResource.amount = 0;
                return takenResource;
            }

            takenResource.amount = amount;
            
            return takenResource;
        }

        public void StoreResource(Resource resourceToStore)
        {
            Resource storedResource = GetResource(resourceToStore.Type);

            if (storedResource == null) 
                storedResources.Add(resourceToStore);
            else 
                storedResource.amount += resourceToStore.amount;
        }
        
    }
}
