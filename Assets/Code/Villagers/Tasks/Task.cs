using System;
using Code.System;
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
        
        public abstract void StartTask();
        public abstract void EndTask();
        public abstract void DoTask();
        public abstract void PauseTask();
        public abstract void AbandonTask();

        public void TakeTask(Villager newWorker, params Action[] taskCompleteActions)
        {
            worker = newWorker;
        
            foreach (Action taskCompleted in taskCompleteActions) 
                onTaskCompleted += taskCompleted;
        }

        protected void ThrowResourceOnGround()
        {
            worker.UI.SetResourceIcon(false, worker.Profession.CarriedResource.Type);
            AssetsStorage.I.ThrowResourceOnTheGround(worker.Profession.CarriedResource, worker.transform.position.x);
            worker.Profession.CarriedResource = null;
        }
    }
}
