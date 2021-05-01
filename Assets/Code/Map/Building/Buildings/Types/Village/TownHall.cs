using Code.Map.Resources;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;
using NotImplementedException = System.NotImplementedException;

namespace Code.Map.Building.Buildings.Types.Village
{
    public class TownHall : Workplace
    {
        protected override Task GetNormalTask()
        {
            throw new NotImplementedException();
        }

        protected override Task GetResourceCarryingTask()
        {
            throw new NotImplementedException();
        }

        protected override void AddTaskToDo(Task task)
        {
            throw new NotImplementedException();
        }

        public override void SetAutomatedTask()
        {
            throw new NotImplementedException();
        }

        public override void TakeTaskBackFromWorker(Task task)
        {
            throw new NotImplementedException();
        }

        protected override void FireNormalWorker(Villager worker)
        {
            
        }

        public override void DeliverStoredResources(Resource storedResource)
        {
            throw new NotImplementedException();
        }
    }
}
