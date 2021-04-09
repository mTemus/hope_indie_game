using System;
using System.Collections.Generic;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Resources
{
   public class ResourcesManager : MonoBehaviour
   {
      private static readonly Resource Wood = new Resource(ResourceType.WOOD, 300);
      private static readonly Resource Stone = new Resource(ResourceType.STONE, 300);

      private readonly Dictionary<Task, Resource> reservedResources = new Dictionary<Task, Resource>();
      
      private int maxStorage = 100;
      
      private bool CanWithdraw(Resource resource, int amount) =>
         resource.amount - amount >= 0;

      public Resource GetResourceByType(ResourceType resourceType)
      {
         return resourceType switch {
            ResourceType.WOOD => Wood,
            ResourceType.STONE => Stone,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
         };
      }

      public void StoreResource(ResourceType resource, int amount)
      {
         GetResourceByType(resource).amount += amount;
      }

      public void IncreaseStorage(int value)
      {
         maxStorage += value;
      }
      
      public void ReserveResources(Task tKey, Resource resource)
      {
         Resource current = GetResourceByType(resource.Type);
         current.amount -= resource.amount;
         reservedResources[tKey] = new Resource(resource);
         Debug.LogWarning("Reserved: " + reservedResources[tKey].Type + " for: " + tKey.GetType());
      }

      public Resource WithdrawReservedResource(Task tKey)
      {
         Resource withdrawn = reservedResources[tKey];
         return withdrawn;
      }

      public void ClearReservedResource(Task tKey)
      {
         Debug.LogWarning("Cleared: " + reservedResources[tKey].Type + " for: " + tKey.GetType());
         reservedResources.Remove(tKey);
      }
      
      public int MAXStorage => maxStorage;
   }
}
