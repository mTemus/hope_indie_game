using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using NotImplementedException = System.NotImplementedException;

namespace HopeMain.Code.World.Buildings.Type.Village
{
    public class TownHall : Workplace.Workplace
    {
        public override void Initialize()
        {
            
        }

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

        public override void TakeTaskBackFromWorker(Task task)
        {
            throw new NotImplementedException();
        }

        protected override void FireNormalWorker(Villager worker)
        {
            
        }
    }
}
