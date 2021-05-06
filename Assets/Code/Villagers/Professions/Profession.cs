using System.Collections.Generic;
using Code.AI;
using Code.Map.Building;
using Code.Map.Resources;
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
        private ProfessionData data;
        private Workplace workplace;
        
        private readonly Queue<Task> tasks = new Queue<Task>();

        private Resource carriedResource;
        private Task currentTask;
        private WorkNode currentWorkNode;

        protected Node professionAI;

        public Workplace Workplace => workplace;
        public ProfessionData Data => data;

        public Resource CarriedResource
        {
            get => carriedResource;
            set => carriedResource = value;
        }

        public abstract void Initialize();
        
        private void AbandonTask(Task t)
        {
            t.AbandonTask();
            workplace.TakeTaskBackFromWorker(t);
        }
        
        public void Work()
        {
            currentTask.DoTask();
        }

        public bool GetNewTask()
        {
            if (tasks.Count <= 0) return false;
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

        public void AbandonAllTasks()
        {
            if (!HasWorkToDo()) 
                return;

            AbandonTask(currentTask);
            
            foreach (Task task in tasks) 
                AbandonTask(task);
        }

        public void ClearCurrentTask()
        {
            currentTask = null;
        }

        public void AbandonCurrentTask()
        {
            if (currentTask != null) {
                AbandonTask(currentTask);
                currentTask = null;
            }

            if (carriedResource != null) {
                //TODO: throw resource on the ground and add global task to pick it up
            }
            
            //TODO: should get new task or respond for AI!
        }

        public void OnTaskCompleted()
        {
            currentTask.EndTask();
            currentWorkNode.StartNewTask();
        }

        public void SetWorkplace(Workplace newWorkplace)
        {
            workplace = newWorkplace;
        }

        public void InitializeWorkerAI()
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

        public void InitializeUnemployedAI()
        {
            
        }

        public void SetProfessionData(ProfessionData professionData)
        {
            data = professionData;
        }
        
        public bool HasWorkToDo() =>
            tasks.Count > 0 || currentTask != null;
    }
}
