using System;
using System.Linq;
using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class Sawmill : GatheringWorkplace
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
                    where task is Task_ResourceGathering
                    select task as Task_ResourceGathering)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is Task_ResourceCarrying
                    select task as Task_ResourceCarrying)
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
