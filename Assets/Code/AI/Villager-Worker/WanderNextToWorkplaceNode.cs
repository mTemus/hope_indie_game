using System;
using Code.Villagers.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.AI
{
    public enum WanderNextToWorkplaceNodeState
    {
        DRAW_WANDER_POSITION,
        WANDER,
        END_WANDER,
    }
    
    public class WanderNextToWorkplaceNode : Node
    {
        private readonly Villager worker;
        private WanderNextToWorkplaceNodeState currentState;
        
        private Vector3 nearPosition;
        private float wanderDistance = 6f;

        public WanderNextToWorkplaceNode(Villager worker)
        {
            this.worker = worker;
        }

        public override NodeState Evaluate()
        {
            if (worker.Profession.HasWorkToDo) {
                nearPosition = Vector3.zero;
                currentState = WanderNextToWorkplaceNodeState.DRAW_WANDER_POSITION;
                return NodeState.FAILURE;
            }

            switch (currentState) {
                case WanderNextToWorkplaceNodeState.DRAW_WANDER_POSITION:
                    Vector3 workplacePos = worker.Profession.Workplace.transform.position + worker.Profession.Workplace.Data.EntrancePivot;
                    float newX = Random.Range(workplacePos.x - wanderDistance, workplacePos.x + wanderDistance);
                    nearPosition = new Vector3(newX, workplacePos.y, workplacePos.z);

                    worker.UI.StateText.text = "Wandering";
                    currentState = WanderNextToWorkplaceNodeState.WANDER;
                    break;
                
                case WanderNextToWorkplaceNodeState.WANDER:
                    if (worker.MoveTo(nearPosition, 1.3f)) 
                        currentState = WanderNextToWorkplaceNodeState.END_WANDER;
                    break;
                
                case WanderNextToWorkplaceNodeState.END_WANDER:
                    nearPosition = Vector3.zero;
                    currentState = WanderNextToWorkplaceNodeState.DRAW_WANDER_POSITION;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return NodeState.RUNNING;
        }
    }
}
