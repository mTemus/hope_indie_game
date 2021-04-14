using System.Collections.Generic;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Map.Building
{
    public abstract class Workplace : Building
    {
        protected readonly List<Profession> workers = new List<Profession>();
        protected readonly List<Profession> workersWithoutTasks = new List<Profession>();
        protected readonly List<Task> tasksToDo = new List<Task>();
        protected readonly List<Task> waitingTasks = new List<Task>();
        
        [Header("Hire event")]
        [SerializeField] protected UnityEvent OnWorkerHired;

        private WorkplaceProperties properties;
        private int haulersCnt;

        #region Building-AI

        //TODO: register free workers
        //TODO: give tasks for free workers when adding a task
        //TODO: register hauler in workers but on different interface, then count limit
        //TODO: on taking task check if task is resourceCarrying, if yes, search for a free porter

        // ----------------------- ADD TASK (task t) TODO: MUST BE ABSTRACT
        // if workersWithoutTasks.len > 0
            // check task type
            // if it's a resourceCarrying task
                // search for a hauler in workersWithoutTasks
                    // if there is one
                        // give him task
                    // if there is no free hauler
                        // give task first free worker
            // if it's not a resource carrying task
                // Check if first free worker is not a hauler
                    // if he's not, give him the task and unregister him
                    // if he is, check if freeWorkers list len > 1
                        // if yes, check next worker
                        // if there is no free worker other than hauler, add task to task list
        // if workersWithoutTasks.len == 0
            // add task to list
                
        // ----------------------- GET TASK (ProfessionType workerProfession) TODO: MUST BE ABSTRACT
        // if workerProfession == hauler
            // GetResourceCarryingTask
        // else
            // if GetNormalTask != null
                // return GetNormalTask
            // else
                // if haulerCnt > 0
                    // return null
                // else
                    // return GetResourceCarrying Task

        // ----------------------- REGISTER WORKER WITHOUT TASK (Profession worker)
        // Task t = GetTask(worker.Type)
        // if t == null
            // register worker as without task
        // else
            // worker.addTask(t)
                    
        // ----------------------- ADD WORKER (Profession worker) TODO: must be abstract in fact of different type of tasks in workplaces
        // workers.add(worker)
        // register free worker
        // create automatic task or wait for task
        
        // ----------------------- ADD WORKER HAULER (Profession hauler)
        // workers.add(hauler)
        // haulersCnt++
        
        // ----------------------- Bool CAN HIRE HAULERS
        // haulersCnt < properties.haulers
            
        //TODO: store different resources
        //TODO: add resources limit
            //TODO: if resource get its limit at storage, throw it on the ground, 
                //TODO: set decay rate to not choke processor
                //TODO: when stored resource reach limit, don't create new gathering task
                //TODO: when limit is over, iterate through workers without tasks and create gathering tasks for them

        #endregion

        #region Tasks

        protected abstract Task GetNormalTask();
        protected abstract Task GetResourceCarryingTask();
        protected abstract void AddTaskToDo(Task task);
        
        // Override this method to add new task when new worker is hired,
        // Fill only if workplace has automated tasks like gathering or production       
        // TODO: find other invoking option, because invoking everytime, even empty method feels dumb
        public abstract void SetAutomatedTask();
                
        public Task GetTask(Profession worker)
        {
            if (tasksToDo.Count == 0) 
                return null;
        
            if (worker.Type == ProfessionType.WorkplaceHauler) 
                return GetResourceCarryingTask();
                    
            Task normalTask = GetNormalTask();
                    
            // TODO: Change this, because player should have an option in building UI to prioritize carryingResource tasks by normal workers
            if (normalTask == null) 
                return haulersCnt == 0 ? null : GetResourceCarryingTask();
                    
            return normalTask;
        }
            
        public void SetTaskWaiting(Task waitingTask)
        {
            waitingTasks.Add(waitingTask);
        }

        public void SetTaskReady(Task readyTask)
        { 
            waitingTasks.Remove(readyTask);
            AddTaskToDo(readyTask);
        }
                
        #endregion
        
        #region Workers

        public void RegisterWorkerWithoutTask(Profession worker)
        {
            Task task = GetTask(worker);

            if (task == null) 
                workersWithoutTasks.Add(worker);
            else 
                worker.AddTask(task);
        }
        
        protected void UnregisterWorkerWithoutTask(Profession worker)
        {
            workersWithoutTasks.Remove(worker);
        }

        public void HireWorker(Profession worker)
        {
            workers.Add(worker);
            OnWorkerHired?.Invoke();
            RegisterWorkerWithoutTask(worker);
        }
        
        public void HireHauler(Profession hauler)
        {
            workers.Add(hauler);
            RegisterWorkerWithoutTask(hauler);
            haulersCnt++;
        }

        public void FireWorker(Profession profession)
        {
            //TODO:
        }

        public void FireHauler(Profession profession)
        {
            
        }
        
        //TODO: fire hauler
        //TODO: fire worker

        public bool CanHireHauler() =>
            haulersCnt < properties.Haulers;

        public bool CanHireWorker() =>
            workers.Count - haulersCnt < properties.Workers;
        
        #endregion

        public WorkplaceProperties Properties
        {
            get => properties;
            set => properties = value;
        }
    }
}
