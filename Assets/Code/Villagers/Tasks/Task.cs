using System;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public abstract class Task
    {
        protected int TaskPriority;
        protected Vector3 TaskPosition;
        protected Villager Worker;
        protected Action OnTaskCompleted;
        
        public abstract void OnTaskStart();
        public abstract void OnTaskEnd();
        public abstract void DoTask();
        public abstract void OnTaskPause();

        public void OnTaskTaken(Villager newWorker, params Action[] taskCompleteActions)
        {
            Worker = newWorker;
        
            foreach (Action taskCompleted in taskCompleteActions) 
                OnTaskCompleted += taskCompleted;
        }
        
        //TODO: on task abandoned
    }
}
