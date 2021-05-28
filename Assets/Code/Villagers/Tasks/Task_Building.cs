using Code.Map.Building;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class Task_Building : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;

        public bool ResourcesDelivered { get; private set; }
        
        public Task_Building(Vector3 taskPosition, Construction construction)
        {
            this.taskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<Building>().Data.EntrancePivot;
        }
        
        public override void StartTask()
        {
            
        }

        public override void EndTask()
        {
            state = TaskState.COMPLETED;
        }

        public override void DoTask()
        {
            if (!worker.MoveTo(constructionPosition)) return;
            if (!construction.Construct()) return;
            onTaskCompleted.Invoke();
            construction.CleanAfterConstruction();
        }

        public void SetResourcesAsDelivered()
        {
            ResourcesDelivered = true;
            onTaskSetReady.Invoke(this);
        }
        
        public override void PauseTask()
        {
        }
    }
}
