using Code.Map.Building;
using UnityEngine;

namespace Code.Villagers.Tasks
{
   public class BuildingTask : Task
    {
        private readonly Construction construction;
        public bool ResourcesDelivered;
        public BuildingTask(int taskPriority, Vector3 taskPosition, Construction construction)
        {
            TaskPriority = taskPriority;
            TaskPosition = taskPosition;
            this.construction = construction;
        }
        
        public override void OnTaskStart()
        {
            
        }

        public override void OnTaskEnd()
        {
            
        }

        public override void DoTask()
        {
            if (!construction.Construct()) return;
            construction.SendMessage("CleanAfterConstruction");
            OnTaskCompleted?.Invoke();
        }

    }
}
