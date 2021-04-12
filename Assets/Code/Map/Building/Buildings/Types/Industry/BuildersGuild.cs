using System.Linq;
using Code.Resources;
using Code.System;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;

namespace Code.Map.Building.Buildings.Types.Industry
{
    public class BuildersGuild : Workplace
    {
        #region Tasks

        protected override Task GetNormalTask()
        {
            return (from task in waitingTasks 
                    where task.GetType() == typeof(BuildingTask) 
                    select (BuildingTask) task)
                .FirstOrDefault(btt => btt.ResourcesDelivered);
        }

        protected override Task GetResourceCarryingTask()
        {
            return (from task in tasksToDo
                    where task.GetType() == typeof(ResourceCarryingTask)
                    select (ResourceCarryingTask) task)
                .FirstOrDefault();
        }
        
        private void CreateResourceCarryingTask(Resource requiredResource, Construction construction)
        {
            ResourceCarryingTask rct = new ResourceCarryingTask(requiredResource, construction.GetComponent<Building>(),
                construction.AddResources, true);
            
            Managers.Instance.Resources.ReserveResources(rct, requiredResource);
            AddTaskToDo(rct);
        }

        protected override void AddTaskToDo(Task task)
        {
            if (task.GetType() == typeof(BuildingTask)) {
                waitingTasks.Add(task);
                return;
            }

            if (workersWithoutTasks.Count > 0) {
                foreach (Profession worker in workersWithoutTasks) {
                    if (Properties.Haulers <= 0) {
                        worker.AddTask(task);
                        UnregisterWorkerWithoutTask(worker);
                        return;
                    }

                    if (worker.Type != ProfessionType.WorkplaceHauler) continue;
                    worker.AddTask(task);
                    UnregisterWorkerWithoutTask(worker);
                    return;
                }
                
                tasksToDo.Add(task);
            }
            else {
                tasksToDo.Add(task);
            }
        }

        public override void SetAutomatedTask()
        {
            // No automated task at builders guild, at least yet.
        }
        
        public void CreateBuildingTask(Construction construction, BuildingData buildingData)
        {
            BuildingTask bt = new BuildingTask(0, construction.transform.position + construction.PositionOffset, construction);
            construction.SetBuildingTask(bt);
            AddTaskToDo(bt);

            foreach (Resource resource in buildingData.RequiredResources) 
                CreateResourceCarryingTask(resource, construction);
        }

        #endregion
    }
}
