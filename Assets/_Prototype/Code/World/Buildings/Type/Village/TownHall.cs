using System;
using _Prototype.Code.AI.Villagers.Tasks;
using _Prototype.Code.Characters.Villagers.Entity;

namespace _Prototype.Code.World.Buildings.Type.Village
{
    public class TownHall : Workplaces.Workplace
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
