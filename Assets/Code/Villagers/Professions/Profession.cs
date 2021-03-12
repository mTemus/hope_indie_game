using System.Collections.Generic;
using Code.Resources;
using Code.Villagers.AI;
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
        protected Node WorkNode;
        private readonly Queue<Task> tasks = new Queue<Task>();
        
        private Resource carriedResource;
        private Task currentTask;
        public void DoWork()
        {
            //TODO: move this to work node
            if (currentTask == null) {
                currentTask = tasks.Dequeue();
                currentTask.OnTaskStart();
            }

            currentTask.DoTask();
        }

        public void GetTask(Task task)
        {
            tasks.Enqueue(task);
        }

        public void OnTaskCompleted()
        {
            currentTask.OnTaskEnd();
            currentTask = null;
            // change boolean in work node to inform about ended task

        }
        
        public Resource CarriedResource
        {
            get => carriedResource;
            set => carriedResource = value;
        }
    }
}
