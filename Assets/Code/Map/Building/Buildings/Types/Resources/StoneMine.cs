using System.Linq;
using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class StoneMine : GatheringWorkplace
    {
        public override void Initialize()
        {
            gatheringResourceType = ResourceType.STONE;
            
            onWorkerHired.AddListener(CreateResourceGatheringTask);
            onHaulerHired.AddListener(TakeTasksBackFromWarehouse);
            Storage.onResourceStored.AddListener(DeliverStoredResources);
            Storage.onResourceLimitReach.AddListener(StopAllTasks);
        }

        #region Resources
        
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
            throw new global::System.NotImplementedException();
        }

        public void EnterWorkplace(Villager worker)
        {
            
            
            
            
        }

        #endregion
    }
}
