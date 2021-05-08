using Code.Villagers.Entity;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Building.Workplaces
{
    public abstract class ServicesWorkplace : Workplace
    {
        #region Workers

        public override void HireWorker(Villager worker)
        {
            HireNormalWorker();
            worker.Profession.Workplace = this;
            workers.Add(worker);
            ReportWorkerWithoutTask(worker);
            
            Debug.LogWarning(name + " had hired: " + worker.name);
        }
        
        public override bool CanHireHauler() => false;

        #endregion
        
        #region Tasks

        protected override Task GetTask(Villager worker)
        {
            if (tasksToDo.Count == 0) return null;
            return GetNormalTask() ?? GetResourceCarryingTask();
        }
        
        #endregion
        
        
    }
}
