using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.System.Assets;
using HopeMain.Code.World.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace HopeMain.Code.World.Buildings.Modules
{
    public class BuildingStorageModule : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private int resourceLimit;
        
        [Header("Observers")]
        public UnityEvent<Resource> onResourceStored;
        public UnityEvent<Resource> onResourceWithdraw;
        public UnityEvent onResourceLimitReach;
        public UnityEvent onResourceLimitReset;
        
        [Header("Debug")]
        [SerializeField] private List<Resource> resources = new List<Resource>();

        public int ResourceLimit => resourceLimit;
        
        private bool CanWithdraw(ResourceType resourceType, int resourceAmount)
        {
            Resource storedResource = GetResourceByType(resourceType);
            
            if (storedResource == null) return false;
            return storedResource.amount >= resourceAmount;
        }

        private Resource GetResourceByType(ResourceType type) =>
            resources.FirstOrDefault(resource => resource.Type == type);
        
        public void StoreResource(Resource newResource)
        {
            Resource storedResource = GetResourceByType(newResource.Type);

            if (storedResource != null) {
                int newAmount = storedResource.amount + newResource.amount;

                if (newAmount > storedResource.Limit) {
                    int overflow = newAmount - storedResource.Limit;
                    storedResource.amount = newAmount - overflow;
                    
                    Resource overflowResource = new Resource(storedResource.Type, overflow);
                    AssetsStorage.I.ThrowResourceOnTheGround(overflowResource, GetComponent<Building>().PivotedPosition.x);
                    onResourceLimitReach.Invoke();
                    return;
                }
                
                storedResource.amount += newResource.amount;
            }
            else {
                resources.Add(new Resource(newResource.Type, newResource.amount, resourceLimit));
            }
            
            // Debug.LogWarning("Stored: " + newResource.amount + " " + newResource.Type + " in " + name);
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

            if (withdrawnResource.amount <= resourceLimit / 2) {
                onResourceLimitReset?.Invoke();
            }
            
            withdrawnResource.amount = resourceAmount;
            return withdrawnResource;
        }
    }
}
