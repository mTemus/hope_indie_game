using System;
using Code.Map.Building;
using Code.Villagers.Brain.Layers;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum Task_Building_State
    {
        GO_TO_CONSTRUCTION,
        BUILD,
    }
    
    public class Task_Building : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;

        private Task_Building_State currentBuildingState;
        
        public Task_Building(Vector3 taskPosition, Construction construction)
        {
            this.taskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<Building>().Data.EntrancePivot;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.Turn(constructionPosition);
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentBuildingState = Task_Building_State.GO_TO_CONSTRUCTION;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;

            switch (currentBuildingState) {
                case Task_Building_State.GO_TO_CONSTRUCTION:
                    if (!worker.Brain.Motion.MoveTo(constructionPosition)) return;
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    currentBuildingState = Task_Building_State.BUILD;
                    break;
                
                case Task_Building_State.BUILD:
                    if (!construction.Construct()) return;
                    onTaskCompleted.Invoke();
                    construction.CleanAfterConstruction();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void End()
        {
            flag = TaskFlag.COMPLETED;
        }
        
        public override void Pause() {}
        
        public void SetResourcesAsDelivered()
        {
            SetReady();
        }
    }
}
