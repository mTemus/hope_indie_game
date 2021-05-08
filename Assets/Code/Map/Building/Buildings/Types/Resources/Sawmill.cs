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
        protected override void Initialize()
        {
            gatheringResourceType = ResourceType.WOOD;
            onWorkerHired.AddListener(CreateResourceGatheringTask);
            onHaulerHired.AddListener(TakeTasksBackFromWarehouse);
        }
        
        #region Tasks

        protected override Task GetNormalTask() 
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceGatheringTask
                    select task as ResourceGatheringTask)
                .FirstOrDefault();

            RemoveTaskFromTodoList(rct);
            return rct;
        }

        protected override Task GetResourceCarryingTask()
        {
            Task rct = (from task in tasksToDo
                    where task is ResourceCarryingTask
                    select task as ResourceCarryingTask)
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
