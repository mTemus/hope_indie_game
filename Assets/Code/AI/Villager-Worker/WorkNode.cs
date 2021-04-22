using System;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.AI
{
    public enum WorkNodeState
    {
        GO_TO_WORKPLACE,
        GET_TASK_TO_DO,
        DO_CURRENT_TASK,
        PAUSE_TASK_,
        ABANDON_TASK,
        REPORT_NO_TASK
    }
    
    public class WorkNode : Node
    {
        private readonly Profession profession;
        private readonly Villager villager;
        
        private WorkNodeState currentState = WorkNodeState.GO_TO_WORKPLACE;

        public WorkNode(Profession profession)
        {
            this.profession = profession;
            villager = profession.GetComponent<Villager>();
        }
        
        public override NodeState Evaluate()
        {
            switch (currentState) {
                case WorkNodeState.GO_TO_WORKPLACE:
                    Vector3 workplacePos = profession.Workplace.transform.position + profession.Workplace.Data.EntrancePivot;
                    villager.MoveTo(workplacePos);

                    if (villager.IsOnPosition(workplacePos)) 
                        currentState = WorkNodeState.GET_TASK_TO_DO;
                    
                    state = NodeState.RUNNING;
                    break;
                
                case WorkNodeState.GET_TASK_TO_DO:
                    if (profession.GetNewTask()) {
                        currentState = WorkNodeState.DO_CURRENT_TASK;
                        state = NodeState.RUNNING;
                    }
                    else {
                        currentState = WorkNodeState.REPORT_NO_TASK;
                        state = NodeState.RUNNING;
                    }

                    break;
                
                case WorkNodeState.DO_CURRENT_TASK:
                    profession.Work();
                    state = NodeState.RUNNING;
                    break;

                case WorkNodeState.REPORT_NO_TASK:
                    currentState = WorkNodeState.GO_TO_WORKPLACE;

                    // TODO: call getting tasks from workplace or register as free

                    state = NodeState.FAILURE;
                    break;
                
                case WorkNodeState.PAUSE_TASK_:
                    profession.PauseCurrentTask();
                    state = NodeState.SUCCESS;
                    break;
                
                case WorkNodeState.ABANDON_TASK:
                    profession.AbandonCurrentTask();
                    currentState = WorkNodeState.GO_TO_WORKPLACE;
                    state = NodeState.FAILURE;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            villager.UI.StateText.text = "Working: " + currentState;
            return state;
        }

        public void StartNewTask() =>
            currentState = WorkNodeState.GET_TASK_TO_DO;

        public void PauseCurrentTask() =>
            currentState = WorkNodeState.PAUSE_TASK_;

        public void AbandonCurrentTask() =>
            currentState = WorkNodeState.ABANDON_TASK;
    }
}
