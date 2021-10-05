using System.Linq;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System;
using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.World.Buildings.Type.Resources
{
    public class StoneMine : Gathering
    {
        public override void Initialize()
        {
            gatheringResourceType = ResourceType.STONE;
            
            onWorkerHired.AddListener(CreateSpotResourceGatheringTask);
            onHaulerHired.AddListener(TakeTasksBackFromWarehouse);
            Storage.onResourceStored.AddListener(DeliverStoredResources);
            Storage.onResourceLimitReach.AddListener(StopAllTasks);

            Vector3 myPosition = transform.position;

            gatheringResource = Managers.I.Areas
                .GetAreaByCoords(myPosition)
                .GetClosestResourceToGatherByType(myPosition, ResourceType.STONE);
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
