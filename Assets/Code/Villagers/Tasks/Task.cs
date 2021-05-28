using System;
using Code.System;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum TaskState
    {
        NEW,
        WAITING,
        RUNNING,
        INTERRUPTED,
        PAUSED,
        ABANDONED,
        COMPLETED
    }

    public abstract class Task
    {
        protected Vector3 taskPosition;
        protected Villager worker;

        public TaskState state = TaskState.NEW;
        
        public Action onTaskCompleted;
        public Action onTaskCancel;
        public Action<Task> onTaskSetReady;
        
        public abstract void StartTask();
        public abstract void EndTask();
        public abstract void DoTask();
        public abstract void PauseTask();

        public virtual void AbandonTask()
        {
            state = TaskState.ABANDONED;
            worker.Profession.Workplace.TakeTaskBackFromWorker(this);
        }

        public void TakeTask(Villager newWorker, params Action[] taskCompleteActions)
        {
            worker = newWorker;
        
            foreach (Action taskCompleted in taskCompleteActions) 
                onTaskCompleted += taskCompleted;
        }

        protected void ThrowResourceOnGround()
        {
            worker.UI.ClearResourceIcon();
            AssetsStorage.I.ThrowResourceOnTheGround(worker.Profession.CarriedResource, worker.transform.position.x);
            worker.Profession.CarriedResource = null;
        }
    }
}