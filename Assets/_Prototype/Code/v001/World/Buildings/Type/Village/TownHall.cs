using System;
using _Prototype.Code.v001.AI.Villagers.Tasks;
using _Prototype.Code.v001.Characters.Villagers.Entity;

namespace _Prototype.Code.v001.World.Buildings.Type.Village
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
