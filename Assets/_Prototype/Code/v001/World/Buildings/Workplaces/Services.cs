using _Prototype.Code.v001.AI.Villagers.Tasks;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings.Workplaces
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Services : Workplaces.Workplace
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
