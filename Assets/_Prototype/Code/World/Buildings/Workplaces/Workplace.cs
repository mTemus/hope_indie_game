using System.Collections.Generic;
using _Prototype.Code.AI.Villagers.Tasks;
using _Prototype.Code.Characters.Villagers.Entity;
using _Prototype.Code.Characters.Villagers.Professions;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototype.Code.World.Buildings.Workplaces
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Workplace : Building
    {
        [Header("Workplace Properties")]
        [SerializeField] private Properties properties;

        [Header("Workplace events")]
        public UnityEvent workerHired;
        public UnityEvent haulerHired;
        public UnityEvent workerEnter;
        
        protected readonly List<Villager> workers = new List<Villager>();
        protected readonly List<Villager> workersWithoutTasks = new List<Villager>();
        protected readonly List<Task> tasksToDo = new List<Task>();
        
        protected int haulersCnt;

        #region Workplace-AI

        // ----------------------- ADD TASK (task t)
        // Method adds task to workplace's "tasksToDo" list to which workers get access and take their work from.
        // Method should be implemented according to workplace-type and should take into account two types of tasks:
            // "resource carrying tasks" handled mostly by haulers
            // "normal tasks" handled by other workers in workplace
        
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
                
        // ----------------------- GET TASK (ProfessionType workerProfession)
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
                    
        // ----------------------- ADD WORKER (Profession worker)
        // workers.add(worker)
        // register free worker
        // create automatic task or wait for task
        
        // ----------------------- ADD WORKER HAULER (Profession hauler)
        // workers.add(hauler)
        // haulersCnt++
        
        // ----------------------- Bool CAN HIRE HAULERS
        // haulersCnt < properties.haulers


        #endregion
        
        #region Tasks

        protected abstract Task GetNormalTask();
        protected abstract Task GetResourceCarryingTask();
        protected abstract void AddTaskToDo(Task task);
        
        protected void GiveTaskToWorker(Villager worker, Task task)
        {
            worker.Brain.Work.AddTask(task);
            task.Take(worker, worker.Brain.Work.CompleteTask);
            UnregisterWorkerWithoutTask(worker);
        }

        protected void RemoveTaskFromTodoList(Task t)
        {
            if (t != null) 
                tasksToDo.Remove(t);
        }
        
        protected virtual Task GetTask(Villager worker)
        {
            if (tasksToDo.Count == 0) 
                return null;
        
            if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                return GetResourceCarryingTask();
                    
            Task normalTask = GetNormalTask();
            
            // TODO: Change this, because player should have an option in building UI to prioritize carryingResource tasks by normal workers
            if (normalTask == null) 
                return HasHiredHaulers ?  GetResourceCarryingTask() : null;
                    
            return normalTask;
        }
            
        public abstract void TakeTaskBackFromWorker(Task task);

        public bool RemoveTaskToDo(Task task) =>
            tasksToDo.Remove(task);

        public void SetTaskReady(Task readyTask)
        { 
            Debug.Log("Setting " + readyTask.GetType().Name + " as ready.");
            readyTask.flag = TaskFlag.Ready;
        }

        #endregion
        
        #region Workers
        
        public bool CanHireHauler => haulersCnt < properties.Haulers;
        public bool HasHiredHaulers => haulersCnt > 0;
        public bool CanHireWorker => workers.Count - haulersCnt < properties.Workers;
        
        protected void ReportWorkerWithoutTask(Villager worker)
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
        
        protected void HireNormalWorker()
        {
            workerHired?.Invoke();
        }
        
        private void HireHauler()
        {
            haulerHired?.Invoke();
            haulersCnt++;
        }
        
        private void UnregisterWorkerWithoutTask(Villager worker)
        {
            workersWithoutTasks.Remove(worker);
        }

        //TODO: this should be rather in a building.cs class or somewhere
        public void WorkerEntersWorkplace(Villager worker)
        {
            MoveWorkerAway_tmp(worker.gameObject);
        }

        private void MoveWorkerAway_tmp(GameObject workerGO)
        {
            workerGO.transform.position = new Vector3(PivotedPosition.x, 100f, workerGO.transform.position.z);
        }

        public virtual void HireWorker(Villager worker)
        {
            if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                HireHauler();
            else 
                HireNormalWorker();

            worker.Profession.Workplace = this;
            workers.Add(worker);
            // Debug.LogWarning(name + " had hired: " + worker.name);
        }

        public virtual void FireWorker(Villager worker)
        {
            if (worker.Profession.Data.Type == ProfessionType.WorkplaceHauler) 
                FireHauler(worker);
            else 
                FireNormalWorker(worker);
            
            workers.Remove(worker);
        }

        protected virtual void FireHauler(Villager worker)
        {
            haulersCnt--;
        }

        protected abstract void FireNormalWorker(Villager worker);
        
        #endregion
    }
}
