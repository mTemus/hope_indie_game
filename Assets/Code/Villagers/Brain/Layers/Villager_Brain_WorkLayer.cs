using System.Collections.Generic;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Tasks;

namespace Code.Villagers.Brain.Layers
{
    public class Villager_Brain_WorkLayer : BrainLayer
    {
        private readonly Queue<Task> tasks = new Queue<Task>();
        
        private Task currentTask;
        
        public Resource CarriedResource { get; set; }
        public bool HasWorkToDo => tasks.Count > 0 || currentTask != null || currentTask != null && currentTask.flag != TaskFlag.COMPLETED;
        public bool TaskComplete => currentTask.flag == TaskFlag.COMPLETED;
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;

        /// Called in Worker Behaviour Tree
        public void Work()
        { 
            currentTask.Execute();
        }
       
        #region Tasks
        
        private void AbandonTask(Task task)
        {
            task.Abandon();
        }
        
        /// Called in Worker Behaviour Tree
        public bool GetNewTask()
        {
            if (tasks.Count <= 0) {
                currentTask = null;
                return false;
            }
            
            currentTask = tasks.Dequeue();
            currentTask.Start();
            return true;
        }
        
        public void AddTask(Task task)
        {
            tasks.Enqueue(task);
        }

        public void PauseCurrentTask()
        {
            currentTask.Pause();
            AddTask(currentTask);
            currentTask = null;
        }

        public void AbandonCurrentTask()
        {
            if (currentTask != null) {
                AbandonTask(currentTask);
                currentTask = null;
            }

            if (!IsCarryingResource) return;
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

        public void InterruptTask()
        {
            currentTask.Interrupt();
            AddTask(currentTask);
            currentTask = null;
        }
        
        public void CompleteTask()
        {
            currentTask.End();
        }
        
        #endregion
    }
}
