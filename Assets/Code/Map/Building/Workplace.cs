using System.Collections.Generic;
using Code.Map.Resources;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Map.Building
{
    public abstract class Workplace : Building
    {
        [Header("Workplace Properties")]
        [SerializeField] private WorkplaceProperties properties;

        [Header("Workplace hire event")]
        [SerializeField] protected UnityEvent onWorkerHired;
        [SerializeField] protected UnityEvent onHaulerHired;
        
        protected readonly List<Villager> workers = new List<Villager>();
        protected readonly List<Villager> workersWithoutTasks = new List<Villager>();
        protected readonly List<Task> tasksToDo = new List<Task>();
        protected readonly List<Task> waitingTasks = new List<Task>();
        
        protected int haulersCnt;

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

        public bool RemoveTaskToDo(Task task) =>
            tasksToDo.Remove(task);
        
        protected void GiveTaskToWorker(Villager worker, Task task)
        {
            worker.Profession.AddTask(task);
            task.TakeTask(worker, worker.Profession.CompleteTask);
            UnregisterWorkerWithoutTask(worker);
        }
        
        // Override this method to add new task when new worker is hired,
        // Fill only if workplace has automated tasks like gathering or production       
        // TODO: Fill in inspector -> OnWorkerHired
        public abstract void SetAutomatedTask();
        
        protected void RemoveTaskFromTodoList(Task t)
        {
            if (t != null) 
                tasksToDo.Remove(t);
        }
        
        private Task GetTask(Villager worker)
        {
            if (tasksToDo.Count == 0) 
                return null;
        
            if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
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
            Debug.Log("Setting " + readyTask.GetType().Name + " as ready.");
            waitingTasks.Remove(readyTask);
            AddTaskToDo(readyTask);
        }

        public abstract void TakeTaskBackFromWorker(Task task);
                
        #endregion
        
        #region Workers

        private void HireNormalWorker()
        {
            onWorkerHired?.Invoke();
        }
        
        private void HireHauler()
        {
            onHaulerHired?.Invoke();
            haulersCnt++;
        }

        public void ReportWorkerWithoutTask(Villager worker)
        {
            Debug.Log("Registering " + worker.name + " as free.");
            
            if (workersWithoutTasks.Contains(worker)) 
                return;
            
            Debug.Log("Getting task for " + worker.name + ".");
            Task task = GetTask(worker);

            if (task == null) {
                Debug.Log("Worker: " + worker.name + " registered as worker without task in: " + name);
                workersWithoutTasks.Add(worker);
            }
            else {
                Debug.Log("Worker: " + worker.name + " got a task: " + task.GetType().Name);
                GiveTaskToWorker(worker, task);
            }
        }
        
        protected void UnregisterWorkerWithoutTask(Villager worker)
        {
            workersWithoutTasks.Remove(worker);
        }

        public void HireWorker(Villager worker)
        {
            if (worker.Profession.Data.Type != ProfessionType.WorkplaceHauler) 
                HireNormalWorker();
            else 
                HireHauler();

            worker.Profession.Workplace = this;
            workers.Add(worker);
            ReportWorkerWithoutTask(worker);
            
            Debug.LogWarning(name + " had hired: " + worker.name);
        }

        public void FireWorker(Villager worker)
        {
            if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                FireHauler(worker);
            else 
                FireNormalWorker(worker);
            
            workers.Remove(worker);
        }

        private void FireHauler(Villager worker)
        {
            haulersCnt--;
        }

        protected abstract void FireNormalWorker(Villager worker);

        public bool CanHireHauler() =>
            haulersCnt < properties.Haulers;

        public bool HasHiredHaulers =>
            haulersCnt > 0;
        
        public bool CanHireWorker() =>
            workers.Count - haulersCnt < properties.Workers;
        
        #endregion

        #region Resources

        public abstract void DeliverStoredResources(Resource storedResource);

        #endregion
    }
}
