using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers.AI.Worker
{
    public class WanderNextToWorkplaceNode : Node
    {
        private Profession profession;
        private Villager villager;
        
        private Vector3 nearPosition;
        private float wanderDistance = 6f;

        public WanderNextToWorkplaceNode(Profession profession)
        {
            this.profession = profession;
            villager = profession.GetComponent<Villager>();
        }

        public override NodeState Evaluate()
        {
            if (profession.HasWorkToDo()) 
                return NodeState.FAILURE;

            if (nearPosition == Vector3.zero) {
                Vector3 workplacePos = profession.Workplace.transform.position + profession.Workplace.EntrancePivot;
                float newX = Random.Range(workplacePos.x - wanderDistance, workplacePos.x + wanderDistance);

                nearPosition = new Vector3(newX, workplacePos.y, workplacePos.z);
            }
            
            villager.MoveTo(nearPosition, 2.5f);

            if (villager.IsOnPosition(nearPosition)) {
                nearPosition = Vector3.zero;
                state = NodeState.FAILURE;
            }
            else {
                state = NodeState.RUNNING;
            }
            
            return state;
        }
    }
}
