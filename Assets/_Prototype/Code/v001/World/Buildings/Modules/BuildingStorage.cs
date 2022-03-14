using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.System.Assets;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototype.Code.v001.World.Buildings.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public class BuildingStorage : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private int resourceLimit;
        
        [Header("Observers")]
        public UnityEvent<Resource> resourceStored;
        public UnityEvent<Resource> resourceWithdraw;
        public UnityEvent resourceLimitReach;
        public UnityEvent resourceLimitReset;
        
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newResource"></param>
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
                    resourceLimitReach.Invoke();
                    return;
                }
                
                storedResource.amount += newResource.amount;
            }
            else {
                resources.Add(new Resource(newResource.Type, newResource.amount, resourceLimit));
            }
            
            // Debug.LogWarning("Stored: " + newResource.amount + " " + newResource.Type + " in " + name);
            resourceStored?.Invoke(resources.FirstOrDefault(resource => resource.Type == newResource.Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceAmount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceAmount"></param>
        /// <returns></returns>
        public Resource WithdrawResourceContinuously(ResourceType resourceType, int resourceAmount)
        {
            Resource withdrawnResource = new Resource(resourceType);

            if (!CanWithdraw(resourceType, resourceAmount)) {
                withdrawnResource.amount = 0;
                return withdrawnResource;
            }

            if (withdrawnResource.amount <= resourceLimit / 2) {
                resourceLimitReset?.Invoke();
            }
            
            withdrawnResource.amount = resourceAmount;
            return withdrawnResource;
        }
    }
}
