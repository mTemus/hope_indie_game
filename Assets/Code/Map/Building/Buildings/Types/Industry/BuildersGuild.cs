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
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
