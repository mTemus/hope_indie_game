using System;
using System.Collections.Generic;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Resources
{
   public class ResourcesManager : MonoBehaviour
   {
      private static Resource _wood = new Resource(ResourceType.WOOD, 300);
      private static Resource _stone = new Resource(ResourceType.STONE, 300);
      
      private Resource GetResourceByType(ResourceType resourceType)
      {
         return resourceType switch {
            ResourceType.WOOD => _wood,
            ResourceType.STONE => _stone,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
         };
      }

      private bool CanWithdraw(Resource resource, int amount) =>
         resource.amount - amount >= 0;

      public void StoreResource(ResourceType resource, int amount)
      {
         GetResourceByType(resource).amount += amount;
      }

      public Resource GetResourceAmount(ResourceType resource, int amount)
      {
         Resource takenResource = new Resource(resource);
         Resource resourceToWithdraw = GetResourceByType(resource);

         if (!CanWithdraw(resourceToWithdraw, amount)) {
            takenResource.amount = 0;
            return takenResource;
         }

         takenResource.amount = amount;
         resourceToWithdraw.amount -= amount;
         return takenResource;
      }

   }
}
