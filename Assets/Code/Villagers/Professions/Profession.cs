using System.Collections.Generic;
using Code.AI;
using Code.Map.Building;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;
using UnityEngine;

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
    
    public abstract class Profession : MonoBehaviour
    {
        private readonly Queue<Task> tasks = new Queue<Task>();

        private WorkNode currentWorkNode;
        private Task currentTask;

        protected Node professionAI;

        public Workplace Workplace { get; set; }
        public Resource CarriedResource { get; set; }
        public ProfessionData Data { get; set; }
        public bool HasWorkToDo =>
            tasks.Count > 0 || currentTask != null;
        
        #region AI

        public abstract void Initialize();

        public void Work()
        {
            currentTask.DoTask();
        }

        protected void InitializeWorkerAI()
        {
            Villager worker = GetComponent<Villager>();
            
            WanderNextToWorkplaceNode wanderNextToWorkplace = new WanderNextToWorkplaceNode(worker);
            CanWorkNode canWork = new CanWorkNode();
            WorkNode workNode = new WorkNode(worker);

            Sequence doTasks = new Sequence(new List<Node>{ canWork, workNode });
            Selector workerAI = new Selector(new List<Node> { wanderNextToWorkplace, doTasks });

            professionAI = workerAI;
            currentWorkNode = workNode;
        }

        protected void InitializeUnemployedAI()
        {
            
        }
        
        #endregion
        
        #region Tasks

        private void AbandonTask(Task t)
        {
            t.AbandonTask();
            Workplace.TakeTaskBackFromWorker(t);
        }
        
        public bool GetNewTask()
        {
            if (tasks.Count <= 0) {
                currentTask = null;
                return false;
            }
            
            currentTask = tasks.Dequeue();
            currentTask.StartTask();
            return true;
        }

        public void AddTask(Task task)
        {
            tasks.Enqueue(task);
        }

        public void PauseCurrentTask()
        {
            currentTask.PauseTask();
            tasks.Enqueue(currentTask);
            currentTask = null;
        }

        public void AbandonCurrentTask()
        {
            if (currentTask != null) {
                AbandonTask(currentTask);
                currentTask = null;
            }

            if (CarriedResource == null) return;
            AssetsStorage.I.ThrowResourceOnTheGround(CarriedResource, transform.position.x);
            CarriedResource = null;
        }
        
        public void AbandonAllTasks()
        {
            if (!HasWorkToDo) 
                return;

            AbandonTask(currentTask);
            
            foreach (Task task in tasks) 
                AbandonTask(task);
        }
        
        public void CompleteTask()
        {
            currentTask.EndTask();
            currentWorkNode.StartNewTask();
        }

        #endregion
    }
}
