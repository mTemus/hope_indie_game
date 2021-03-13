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
        
        protected Node ProfessionAI;
        protected WorkNode WorkNode;
        private readonly Queue<Task> tasks = new Queue<Task>();
        
        private Building workplace;

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
        
        public Building Workplace => workplace;

        public ProfessionType Type => type;

        public Resource CarriedResource
        {
            get => carriedResource;
            set => carriedResource = value;
        }
    }
}
