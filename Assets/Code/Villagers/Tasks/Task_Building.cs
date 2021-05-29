using Code.Map.Building;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class Task_Building : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;
        
        public Task_Building(Vector3 taskPosition, Construction construction)
        {
            this.taskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<Building>().Data.EntrancePivot;
        }
        
        public override void Start()
        {
            
        }

        public override void End()
        {
            state = TaskState.COMPLETED;
        }

        public override void Execute()
        {
            state = TaskState.RUNNING;
            
            if (!worker.MoveTo(constructionPosition)) return;
            if (!construction.Construct()) return;
            onTaskCompleted.Invoke();
            construction.CleanAfterConstruction();
        }

        public void SetResourcesAsDelivered()
        {
            SetReady();
        }
        
        public override void Pause()
        {
        }
    }
}
