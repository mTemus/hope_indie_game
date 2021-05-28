using System.Collections.Generic;
using Code.Map.Building.Workplaces;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Tasks;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using Task = Code.Villagers.Tasks.Task;

namespace Code.Villagers.Professions
{
    public enum ProfessionType
    {
        Unemployed,
        Builder,
        Lumberjack,
        WorkplaceHauler,
        GlobalHauler,
    }
    
    public abstract class Villager_Profession : MonoBehaviour
    {
        private readonly Queue<Task> tasks = new Queue<Task>();

        private Task currentTask;
        public BehaviourTreeOwner BTO;
        
        public Workplace Workplace { get; set; }
        public Resource CarriedResource { get; set; }
        public Villager_ProfessionData Data { get; set; }
        public bool HasWorkToDo => tasks.Count > 0 || currentTask != null || currentTask != null && currentTask.state != TaskState.COMPLETED;
        public bool TaskComplete => currentTask.state == TaskState.COMPLETED;
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;

        
        #region AI

        public abstract void Initialize();

        // public void Work()
        // {
        //     currentTask.DoTask();
        // }

        protected void InitializeWorkerAI()
        {
            //TODO: this to public method
            
            BTO = GetComponent<BehaviourTreeOwner>();
        }

        protected void InitializeUnemployedAI()
        {
            
        }

        // //TODO: this to brain
        // public void ClearAIComponents()
        // {
        //     BTO.StopBehaviour();
        //     BTO.blackboard.variables.Clear();
        //     BTO.graph = null;
        // }
        //
        #endregion
        
        #region Tasks

        // private void AbandonTask(Task t)
        // {
        //     t.AbandonTask();
        //     Workplace.TakeTaskBackFromWorker(t);
        // }
        //
        // public bool GetNewTask()
        // {
        //     if (tasks.Count <= 0) {
        //         currentTask = null;
        //         return false;
        //     }
        //     
        //     currentTask = tasks.Dequeue();
        //     currentTask.StartTask();
        //     return true;
        // }
        //
        // public void AddTask(Task task)
        // {
        //     tasks.Enqueue(task);
        // }
        //
        // public void PauseCurrentTask()
        // {
        //     currentTask.PauseTask();
        //     tasks.Enqueue(currentTask);
        //     currentTask = null;
        // }
        //
        // public void AbandonCurrentTask()
        // {
        //     if (currentTask != null) {
        //         AbandonTask(currentTask);
        //         currentTask = null;
        //     }
        //
        //     if (!IsCarryingResource) return;
        //     AssetsStorage.I.ThrowResourceOnTheGround(CarriedResource, transform.position.x);
        //     CarriedResource = null;
        // }
        //
        // public void AbandonAllTasks()
        // {
        //     if (!HasWorkToDo) 
        //         return;
        //
        //     AbandonTask(currentTask);
        //     
        //     foreach (Task task in tasks) 
        //         AbandonTask(task);
        // }
        //
        // public void CompleteTask()
        // {
        //     currentTask.EndTask();
        // }

        #endregion
    }
}