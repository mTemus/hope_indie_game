using System.Collections.Generic;
using System.Linq;
using Code.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Map.Building.Buildings.Components
{
    public class BuildingStorageModule : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private int resourceLimit;
        [SerializeField] private UnityEvent<Resource> onResourceStored;
        [SerializeField] private UnityEvent<Resource> onResourceWithdraw;
        
        private readonly List<Resource> resources = new List<Resource>();

        private Resource GetResource(ResourceType type) =>
            resources.FirstOrDefault(resource => resource.Type == type);
        
        public void StoreResource(Resource newResource)
        {
            Resource storedResource = resources.FirstOrDefault(resource => resource.Type == newResource.Type);

            if (storedResource != null) {
                int newAmount = storedResource.amount + newResource.amount;

                if (newAmount > storedResource.Limit) {
                    int overflow = newAmount - storedResource.Limit;
                    storedResource.amount = newAmount - overflow;
                    onResourceStored?.Invoke(storedResource);
                    
                    //TODO: Throw overflow on the ground
                    Resource overflowResource = new Resource(storedResource.Type, overflow);
                }
                else {
                    storedResource.amount += newResource.amount;
                    onResourceStored?.Invoke(storedResource);
                }
            }
            else {
                resources.Add(new Resource(newResource.Type, newResource.amount, 500));
            }
        }

        public Resource WithdrawResource(ResourceType resourceType, int resourceAmount)
        {
            Resource withdrawnResource = new Resource(resourceType);

            if (!CanWithdraw(resourceType, resourceAmount)) {
                withdrawnResource.amount = 0;
                return withdrawnResource;
            }

            Resource storedResource = GetResource(resourceType);
            storedResource.amount -= resourceAmount;
            withdrawnResource.amount = resourceAmount;
            
            return withdrawnResource;
        }

        private bool CanWithdraw(ResourceType resourceType, int resourceAmount)
        {
            Resource storedResource = GetResource(resourceType);
            
            if (storedResource == null) 
                return false;

            if (storedResource.amount < resourceAmount) 
                return false;

            return true;
        }

        public int ResourceLimit => resourceLimit;
    }
}