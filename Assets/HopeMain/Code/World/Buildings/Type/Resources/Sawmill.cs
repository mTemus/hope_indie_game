using System;
using System.Linq;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Resources;

namespace HopeMain.Code.World.Buildings.Type.Resources
{
    public class Sawmill : Gathering
    {
        public override void Initialize()
        {
            gatheringResourceType = ResourceType.WOOD;
            
            onWorkerHired.AddListener(CreateSingleResourceGatheringTask);
            onHaulerHired.AddListener(TakeTasksBackFromWarehouse);
            Storage.onResourceStored.AddListener(DeliverStoredResources);
            Storage.onResourceLimitReach.AddListener(StopAllTasks);
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
