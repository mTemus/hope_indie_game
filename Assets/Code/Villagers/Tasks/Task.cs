using System;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public abstract class Task
    {
        protected int taskPriority;
        protected Vector3 taskPosition;
        protected Villager worker;
        protected Action onTaskCompleted;
        
        public Action<Task> onTaskSetReady;
        
        public abstract void OnTaskStart();
        public abstract void OnTaskEnd();
        public abstract void DoTask();
        public abstract void OnTaskPause();
        public abstract void OnTaskAbandon();

        public bool CancelTask()
        {
            if (worker == null) 
                return false;

            worker.Profession.CancelCurrentTask();
            return true;
        }
        
        public void OnTaskTaken(Villager newWorker, params Action[] taskCompleteActions)
        {
            worker = newWorker;
        
            foreach (Action taskCompleted in taskCompleteActions) 
                onTaskCompleted += taskCompleted;
        }
    }
}
