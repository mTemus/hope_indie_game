using System.Collections.Generic;
using Code.Map.Building;
using Code.Resources;
using Code.Villagers.AI;
using Code.Villagers.AI.Worker;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Villagers.Professions
{
    public enum ProfessionType
    {
        UNEMPLOYED,
        BUILDER,
    }
    
    public abstract class Profession : MonoBehaviour
    {
        [SerializeField] private ProfessionType type;
        [SerializeField] private Building workplace;
        
        protected Node ProfessionAI;
        protected WorkNode WorkNode;
        private readonly Queue<Task> tasks = new Queue<Task>();

        private Resource carriedResource;
        private Task currentTask;
        
        public void DoWork()
        {
            currentTask.DoTask();
        }

        public bool GetTask()
        {
            if (tasks.Count <= 0) return false;
            currentTask = tasks.Dequeue();
            currentTask.OnTaskStart();
            return true;
        }

        public void AddTask(Task task)
        {
            tasks.Enqueue(task);
        }

        public void PauseCurrentTask()
        {
            currentTask.OnTaskPause();
            tasks.Enqueue(currentTask);
            currentTask = null;
        }

        public void AbandonCurrentTask()
        {
            currentTask = null;
        }

        public void OnTaskCompleted()
        {
            currentTask.OnTaskEnd();
            currentTask = null;
            WorkNode.StartNewTask();
        }

        public void SetWorkplace(Building newWorkplace)
        {
            workplace = newWorkplace;
        }

        public void InitializeWorkerAI()
        {
            TryToGetTaskNode tryToGetTask = new TryToGetTaskNode(this);
            WanderNextToWorkplaceNode wanderNextToWorkplace = new WanderNextToWorkplaceNode(this);
            CanWorkNode canWork = new CanWorkNode();
            WorkNode workNode = new WorkNode(this);

            Selector findTasks = new Selector(new List<Node> { tryToGetTask, wanderNextToWorkplace });
            Selector doTasks = new Selector(new List<Node>{ canWork, workNode });
            Sequence workerAI = new Sequence(new List<Node> { findTasks, doTasks });

            ProfessionAI = workerAI;
            WorkNode = workNode;
            
            Debug.Log("Here " + name);
        }

        public bool HasWorkToDo() =>
            tasks.Count > 0 || currentTask != null;

        public Building Workplace => workplace;

        public ProfessionType Type => type;

        public Resource CarriedResource
        {
            get => carriedResource;
            set => carriedResource = value;
        }
    }
}
