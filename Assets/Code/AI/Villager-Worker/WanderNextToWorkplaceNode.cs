using System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
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
        private readonly Profession profession;
        private readonly Villager villager;
        private WanderNextToWorkplaceNodeState currentState;
        
        private Vector3 nearPosition;
        private float wanderDistance = 6f;

        public WanderNextToWorkplaceNode(Profession profession)
        {
            this.profession = profession;
            villager = profession.GetComponent<Villager>();
        }

        public override NodeState Evaluate()
        {
            if (profession.HasWorkToDo()) {
                nearPosition = Vector3.zero;
                currentState = WanderNextToWorkplaceNodeState.DRAW_WANDER_POSITION;
                return NodeState.FAILURE;
            }

            switch (currentState) {
                case WanderNextToWorkplaceNodeState.DRAW_WANDER_POSITION:
                    Vector3 workplacePos = profession.Workplace.transform.position + profession.Workplace.Data.EntrancePivot;
                    float newX = Random.Range(workplacePos.x - wanderDistance, workplacePos.x + wanderDistance);
                    nearPosition = new Vector3(newX, workplacePos.y, workplacePos.z);

                    villager.UI.StateText.text = "Wandering";
                    currentState = WanderNextToWorkplaceNodeState.WANDER;
                    break;
                
                case WanderNextToWorkplaceNodeState.WANDER:
                    villager.MoveTo(nearPosition, 1.3f);

                    if (villager.IsOnPosition(nearPosition)) 
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
