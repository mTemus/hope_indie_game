using System;
using Code.Villagers.Entity;
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
        private readonly Villager worker;
        
        private WorkNodeState currentState = WorkNodeState.GET_TASK_TO_DO;

        public WorkNode(Villager worker)
        {
            this.worker = worker;
        }
        
        public override NodeState Evaluate()
        {
            switch (currentState) {
                case WorkNodeState.GO_TO_WORKPLACE:
                    if (worker.MoveTo(worker.Profession.Workplace.PivotedPosition)) 
                        currentState = WorkNodeState.GET_TASK_TO_DO;
                    
                    state = NodeState.RUNNING;
                    break;
                
                case WorkNodeState.GET_TASK_TO_DO:
                    if (worker.Profession.GetNewTask()) {
                        Debug.Log(worker.Profession.Data.Type + " got a task.");
                        currentState = WorkNodeState.DO_CURRENT_TASK;
                    }
                    else {
                        Debug.Log(worker.Profession.Data.Type + " got no task. Reporting as free.");
                        
                        //TODO: current AI is shit, so this is a patch, cause worker don't want to take new tasks after
                        //TODO: having done a task
                        worker.Profession.Workplace.ReportWorkerWithoutTask(worker);
                        currentState = WorkNodeState.REPORT_NO_TASK;
                    }
                    
                    state = NodeState.RUNNING;
                    break;
                
                case WorkNodeState.DO_CURRENT_TASK:
                    worker.Profession.Work();
                    state = NodeState.RUNNING;
                    break;

                case WorkNodeState.REPORT_NO_TASK:
                    Debug.LogError("Worker: " + worker.Profession.name + " reporting no tasks!");
                    worker.Profession.Workplace.ReportWorkerWithoutTask(worker);

                    if (worker.Profession.HasWorkToDo) {
                        currentState = WorkNodeState.GET_TASK_TO_DO;
                        state = NodeState.RUNNING;
                    }
                    else {
                        currentState = WorkNodeState.GO_TO_WORKPLACE;
                        state = NodeState.FAILURE;
                    }
                    break;
                
                case WorkNodeState.PAUSE_TASK_:
                    worker.Profession.PauseCurrentTask();
                    state = NodeState.SUCCESS;
                    break;
                
                case WorkNodeState.ABANDON_TASK:
                    worker.Profession.AbandonCurrentTask();
                    currentState = WorkNodeState.GO_TO_WORKPLACE;
                    state = NodeState.FAILURE;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            worker.UI.StateText.text = "Working: " + currentState;
            return state;
        }

        public void StartNewTask()
        {
            Debug.LogWarning("Worker: " + worker.Profession.name + " is starting new task");
            currentState = WorkNodeState.GET_TASK_TO_DO;
        }

        public void PauseCurrentTask() =>
            currentState = WorkNodeState.PAUSE_TASK_;

        public void AbandonCurrentTask() =>
            currentState = WorkNodeState.ABANDON_TASK;
    }
}
