using System;
using System.Collections.Generic;
using HopeMain.Code.AI.Villagers.Tasks;
using UnityEngine;

namespace HopeMain.Code.World.Resources
{
   public class ResourcesManager : MonoBehaviour
   {
      private static readonly Resource Wood = new Resource(ResourceType.WOOD, 300);
      private static readonly Resource Stone = new Resource(ResourceType.STONE, 300);

      private readonly Dictionary<Task, Resource> tasksWaitingForResources = new Dictionary<Task, Resource>();
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

      public void StoreResource(ResourceType resourceType, int amount)
      {
         GetResourceByType(resourceType).amount += amount;

         Dictionary<Task, Resource> tmpWaitingTasks = new Dictionary<Task, Resource>(tasksWaitingForResources);

         foreach (Task waitingTask in tmpWaitingTasks.Keys) {
            Resource awaitedResource = tasksWaitingForResources[waitingTask];

            if (resourceType != awaitedResource.Type) continue;
            if (!CanReserveResource(awaitedResource)) continue;
            ReserveResources(waitingTask, awaitedResource);
            tasksWaitingForResources.Remove(waitingTask);
            waitingTask.onTaskSetReady.Invoke();
         }
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
         Resource reservedResource = reservedResources[tKey];
         GetResourceByType(reservedResource.Type).amount += reservedResource.amount;
         reservedResources.Remove(tKey);
      }

      public bool IsResourceReserved(Task keyTask) =>
         reservedResources.ContainsKey(keyTask);

      public bool CanReserveResource(Resource resource) =>
         GetResourceByType(resource.Type).amount - resource.amount >= 0;

      public bool CanWithdrawReserved(Task t, Resource r) =>
         reservedResources[t].amount - r.amount >= 0;
      
      public void AddWaitingTask(Task t, Resource r)
      {
         tasksWaitingForResources[t] = r;
      }

      public int MAXStorage => maxStorage;
   }
}
