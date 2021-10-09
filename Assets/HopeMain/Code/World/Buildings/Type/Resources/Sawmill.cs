using System;
using System.Linq;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Resources;

namespace HopeMain.Code.World.Buildings.Type.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public class Sawmill : Gathering
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            gatheringResourceType = ResourceType.Wood;
            
            workerHired.AddListener(CreateSingleResourceGatheringTask);
            haulerHired.AddListener(TakeTasksBackFromWarehouse);
            Storage.resourceStored.AddListener(DeliverStoredResources);
            Storage.resourceLimitReach.AddListener(StopAllTasks);
        }
        
        #region Tasks

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

        protected override void FireNormalWorker(Villager worker)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
