using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using NotImplementedException = System.NotImplementedException;

namespace Code.Map.Building.Buildings.Types.Resources
{
    public class Sawmill : Workplace
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

        protected override void FireNormalWorker(Profession worker)
        {
            throw new NotImplementedException();
        }
    }
}
