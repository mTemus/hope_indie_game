using Code.Map.Building;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class BuildingTask : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;

        public bool ResourcesDelivered { get; private set; }
        
        public BuildingTask(int taskPriority, Vector3 taskPosition, Construction construction)
        {
            this.taskPriority = taskPriority;
            this.taskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<Building>().Data.EntrancePivot;
        }
        
        public override void StartTask()
        {
            
        }

        public override void EndTask()
        {
            
        }

        public override void DoTask()
        {
            if (Vector3.Distance(worker.transform.position, constructionPosition) >= 0.1f) {
                worker.MoveTo(constructionPosition);
            }
            else {
                if (!construction.Construct()) return;
                onTaskCompleted.Invoke();
                construction.CleanAfterConstruction();
            }
        }

        public void SetResourcesAsDelivered()
        {
            ResourcesDelivered = true;
            onTaskSetReady.Invoke(this);
        }
        
        public override void PauseTask()
        {
        }

        public override void AbandonTask()
        {
        }
    }
}
