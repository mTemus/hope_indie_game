using System;
using System.Collections.Generic;
using HopeMain.Code.AI.Villagers.Tasks;
using UnityEngine;

namespace HopeMain.Code.World.Resources
{
   /// <summary>
   /// 
   /// </summary>
   public class ResourcesManager : MonoBehaviour
   {
      private static readonly Resource Wood = new Resource(ResourceType.Wood, 300);
      private static readonly Resource Stone = new Resource(ResourceType.Stone, 300);

      private readonly Dictionary<Task, Resource> _tasksWaitingForResources = new Dictionary<Task, Resource>();
      private readonly Dictionary<Task, Resource> _reservedResources = new Dictionary<Task, Resource>();
      
      private int _maxStorage = 100;
      
      private bool CanWithdraw(Resource resource, int amount) =>
         resource.amount - amount >= 0;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="resourceType"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentOutOfRangeException"></exception>
      public Resource GetResourceByType(ResourceType resourceType)
      {
         return resourceType switch {
            ResourceType.Wood => Wood,
            ResourceType.Stone => Stone,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
         };
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="resourceType"></param>
      /// <param name="amount"></param>
      public void StoreResource(ResourceType resourceType, int amount)
      {
         GetResourceByType(resourceType).amount += amount;

         Dictionary<Task, Resource> tmpWaitingTasks = new Dictionary<Task, Resource>(_tasksWaitingForResources);

         foreach (Task waitingTask in tmpWaitingTasks.Keys) {
            Resource awaitedResource = _tasksWaitingForResources[waitingTask];

            if (resourceType != awaitedResource.Type) continue;
            if (!CanReserveResource(awaitedResource)) continue;
            ReserveResources(waitingTask, awaitedResource);
            _tasksWaitingForResources.Remove(waitingTask);
            waitingTask.taskSetReady.Invoke();
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="value"></param>
      public void IncreaseStorage(int value)
      {
         _maxStorage += value;
      }
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="tKey"></param>
      /// <param name="resource"></param>
      public void ReserveResources(Task tKey, Resource resource)
      {
         Resource current = GetResourceByType(resource.Type);
         current.amount -= resource.amount;
         _reservedResources[tKey] = new Resource(resource);
         Debug.LogWarning("Reserved: " + _reservedResources[tKey].Type + " for: " + tKey.GetType());
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="tKey"></param>
      /// <returns></returns>
      public Resource WithdrawReservedResource(Task tKey)
      {
         Resource withdrawn = _reservedResources[tKey];
         return withdrawn;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="tKey"></param>
      public void ClearReservedResource(Task tKey)
      {
         Debug.LogWarning("Cleared: " + _reservedResources[tKey].Type + " for: " + tKey.GetType());
         Resource reservedResource = _reservedResources[tKey];
         GetResourceByType(reservedResource.Type).amount += reservedResource.amount;
         _reservedResources.Remove(tKey);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="keyTask"></param>
      /// <returns></returns>
      public bool IsResourceReserved(Task keyTask) =>
         _reservedResources.ContainsKey(keyTask);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="resource"></param>
      /// <returns></returns>
      public bool CanReserveResource(Resource resource) =>
         GetResourceByType(resource.Type).amount - resource.amount >= 0;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="t"></param>
      /// <param name="r"></param>
      /// <returns></returns>
      public bool CanWithdrawReserved(Task t, Resource r) =>
         _reservedResources[t].amount - r.amount >= 0;
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="t"></param>
      /// <param name="r"></param>
      public void AddWaitingTask(Task t, Resource r)
      {
         _tasksWaitingForResources[t] = r;
      }

      public int MAXStorage => _maxStorage;
   }
}
