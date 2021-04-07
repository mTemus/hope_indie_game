using Code.Map.Building;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class BuildingTask : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;
        public bool ResourcesDelivered;
        
        public BuildingTask(int taskPriority, Vector3 taskPosition, Construction construction)
        {
            TaskPriority = taskPriority;
            TaskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<Building>().Data.EntrancePivot;
        }
        
        public override void OnTaskStart()
        {
            
        }

        public override void OnTaskEnd()
        {
            
        }

        public override void DoTask()
        {
            if (Vector3.Distance(Worker.transform.position, constructionPosition) >= 0.1f) {
                Worker.MoveTo(constructionPosition);
            }
            else {
                if (!construction.Construct()) return;
                construction.SendMessage("CleanAfterConstruction");
                OnTaskCompleted?.Invoke();
            }
        }

        public override void OnTaskPause()
        {
        }
    }
}
