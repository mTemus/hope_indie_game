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
            flag = TaskFlag.COMPLETED;
        }

        public override void Execute()
        {
            flag = TaskFlag.RUNNING;
            
            if (!worker.Brain.Motion.MoveTo(constructionPosition)) return;
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
