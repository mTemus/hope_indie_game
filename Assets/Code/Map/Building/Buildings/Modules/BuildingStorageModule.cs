using System.Collections.Generic;
using System.Linq;
using Code.Map.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Map.Building.Buildings.Modules
{
    public class BuildingStorageModule : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private int resourceLimit;
        [SerializeField] private UnityEvent<Resource> onResourceStored;
        [SerializeField] private UnityEvent<Resource> onResourceWithdraw;
        
        [Header("Debug")]
        [SerializeField] private List<Resource> resources = new List<Resource>();

        public int ResourceLimit => resourceLimit;
        
        private bool CanWithdraw(ResourceType resourceType, int resourceAmount)
        {
            Resource storedResource = GetResourceByType(resourceType);
            
            if (storedResource == null) 
                return false;

            if (storedResource.amount < resourceAmount) 
                return false;

            return true;
        }
        
        public Resource GetResourceByType(ResourceType type) =>
            resources.FirstOrDefault(resource => resource.Type == type);
        
        public void StoreResource(Resource newResource)
        {
            Resource storedResource = resources.FirstOrDefault(resource => resource.Type == newResource.Type);

            if (storedResource != null) {
                int newAmount = storedResource.amount + newResource.amount;

                if (newAmount > storedResource.Limit) {
                    int overflow = newAmount - storedResource.Limit;
                    storedResource.amount = newAmount - overflow;
                    
                    //TODO: Throw overflow on the ground
                    Resource overflowResource = new Resource(storedResource.Type, overflow);
                }
                else {
                    storedResource.amount += newResource.amount;
                }
            }
            else {
                resources.Add(new Resource(newResource.Type, newResource.amount, 500));
            }
            
            Debug.LogWarning("Stored: " + newResource.amount + " " + newResource.Type + " in " + name);
            onResourceStored?.Invoke(resources.FirstOrDefault(resource => resource.Type == newResource.Type));
        }

        public Resource WithdrawResource(ResourceType resourceType, int resourceAmount)
        {
            Resource withdrawnResource = new Resource(resourceType);

            if (!CanWithdraw(resourceType, resourceAmount)) {
                withdrawnResource.amount = 0;
                return withdrawnResource;
            }

            Resource storedResource = GetResourceByType(resourceType);
            storedResource.amount -= resourceAmount;
            withdrawnResource.amount = resourceAmount;
            
            return withdrawnResource;
        }

        public Resource WithdrawResourceContinuously(ResourceType resourceType, int resourceAmount)
        {
            Resource withdrawnResource = new Resource(resourceType);

            if (!CanWithdraw(resourceType, resourceAmount)) {
                withdrawnResource.amount = 0;
                return withdrawnResource;
            }
            
            withdrawnResource.amount = resourceAmount;
            return withdrawnResource;
        }
    }
}
