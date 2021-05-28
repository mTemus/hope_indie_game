using System.Collections.Generic;
using Code.Map.Resources;
using Code.System;
using Code.Villagers.Professions;
using Code.Villagers.Tasks;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_WorkLayer : BrainLayer
    {
        private readonly Queue<Task> tasks = new Queue<Task>();
        
        private Task currentTask;
        
        public Resource CarriedResource { get; set; }
        public bool HasWorkToDo => tasks.Count > 0 || currentTask != null || currentTask != null && currentTask.state != TaskState.COMPLETED;
        public bool TaskComplete => currentTask.state == TaskState.COMPLETED;
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;
        public Villager_Profession Profession { get; set; }

        public override void Initialize(Villager_Brain villagerBrain)
        {
            brain = villagerBrain;
            
        }
        
        public void Work()
        {
            currentTask.DoTask();
        }
        
        
        #region Tasks
        
        private void AbandonTask(Task task)
        {
            task.AbandonTask();
            Profession.Workplace.TakeTaskBackFromWorker(task);
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
            task.state = TaskState.NEW;
        }

        public void PauseCurrentTask()
        {
            currentTask.PauseTask();
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
        
        public void CompleteTask()
        {
            currentTask.EndTask();
        }
        
        #endregion
    }
}
