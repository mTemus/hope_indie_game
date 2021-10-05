using System;
using HopeMain.Code.AI.Villagers.Tasks;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.World.Buildings.Workplace;

namespace HopeMain.Code.World.Buildings.Type.Village
{
    public class TownHall : WorkplaceBase
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
