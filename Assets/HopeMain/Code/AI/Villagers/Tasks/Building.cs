using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.World.Buildings;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public enum BuildingFlag
    {
        GO_TO_CONSTRUCTION,
        BUILD,
    }
    
    public class Building : Task
    {
        private readonly Construction construction;
        private readonly Vector3 constructionPosition;

        private BuildingFlag _flag;
        
        public Building(Vector3 taskPosition, Construction construction)
        {
            this.taskPosition = taskPosition;
            this.construction = construction;
            constructionPosition = construction.transform.position + construction.GetComponent<World.Buildings.Building>().Data.EntrancePivot;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            _flag = BuildingFlag.GO_TO_CONSTRUCTION;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;

            switch (_flag) {
                case BuildingFlag.GO_TO_CONSTRUCTION:
                    if (!worker.Brain.Motion.MoveTo(constructionPosition)) return;
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    _flag = BuildingFlag.BUILD;
                    break;
                
                case BuildingFlag.BUILD:
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
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            flag = TaskFlag.COMPLETED;
        }
        
        public override void Pause() {}
        
        public void SetResourcesAsDelivered()
        {
            SetReady();
        }
    }
}
