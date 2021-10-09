using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.World.Buildings;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum BuildingFlag
    {
        GOToConstruction,
        Build,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class Building : Task
    {
        private readonly Construction _construction;
        private readonly Vector3 _constructionPosition;

        private BuildingFlag _flag;
        
        public Building(Vector3 taskPosition, Construction construction)
        {
            this.taskPosition = taskPosition;
            _construction = construction;
            _constructionPosition = construction.transform.position + construction.GetComponent<World.Buildings.Building>().Data.EntrancePivot;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            _flag = BuildingFlag.GOToConstruction;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.Running;

            switch (_flag) {
                case BuildingFlag.GOToConstruction:
                    if (!worker.Brain.Motion.MoveTo(_constructionPosition)) return;
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    _flag = BuildingFlag.Build;
                    break;
                
                case BuildingFlag.Build:
                    if (!_construction.Construct()) return;
                    taskCompleted.Invoke();
                    _construction.CleanAfterConstruction();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void End()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            flag = TaskFlag.Completed;
        }
        
        public override void Pause() {}
        
        public void SetResourcesAsDelivered()
        {
            //TODO: ???
            SetReady();
        }
    }
}
