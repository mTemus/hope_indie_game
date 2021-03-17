using System;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public abstract class Task
    {
        protected int TaskPriority;
        protected Vector3 TaskPosition;
        protected Villager Worker;
        protected Profession WorkerProfession;
        protected Action OnTaskCompleted;
        
        public abstract void OnTaskStart();
        public abstract void OnTaskEnd();
        public abstract void DoTask();
        public abstract void OnTaskPause();

        public void OnTaskTaken(Villager newWorker, params Action[] taskCompleteActions)
        {
            Worker = newWorker;
            WorkerProfession = Worker.GetComponent<Profession>();
        
            foreach (Action taskCompleted in taskCompleteActions) 
                OnTaskCompleted += taskCompleted;
        }
        
        //TODO: on task abandoned
    }
}
