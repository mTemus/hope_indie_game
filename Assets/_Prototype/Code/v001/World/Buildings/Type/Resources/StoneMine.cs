using System.Linq;
using _Prototype.Code.v001.AI.Villagers.Tasks;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.World.Buildings.Workplaces;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings.Type.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public class StoneMine : Gathering
    {
        
        public override void Initialize()
        {
            gatheringResourceType = ResourceType.Stone;
            
            workerHired.AddListener(CreateSpotResourceGatheringTask);
            haulerHired.AddListener(TakeTasksBackFromWarehouse);
            Storage.resourceStored.AddListener(DeliverStoredResources);
            Storage.resourceLimitReach.AddListener(StopAllTasks);

            Vector3 myPosition = transform.position;

            gatheringResource = Managers.I.Areas
                .GetAreaByCoords(myPosition)
                .GetClosestResourceToGatherByType(myPosition, ResourceType.Stone);
        }

        #region Resources
        
        protected override Task GetNormalTask() 
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceGathering
                    select task as ResourceGathering)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceCarrying
                    select task as ResourceCarrying)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            AddTaskToDo(task);
        }
        
        #endregion

        #region Workers

        protected override void FireNormalWorker(Villager worker) { }

        #endregion
    }
}
