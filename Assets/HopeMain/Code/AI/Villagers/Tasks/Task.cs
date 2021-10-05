using System;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System.Assets;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public enum TaskFlag
    {
        NEW,
        WAITING,
        READY,
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

        public TaskFlag flag = TaskFlag.NEW;
        
        public Action onTaskCompleted;
        public Action onTaskCancel;
        public Action onTaskSetReady;
        
        /// <summary>
        /// Method should check current state of task and execute block of code dependently of it
        /// </summary>
        public abstract void Start();
        
        /// <summary>
        /// Method should execute code for end of current task
        /// </summary>
        public abstract void End();
        
        /// <summary>
        /// Method should execute code of task behaviour
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Method should pause current task because it can't be executed anymore because of some condition/reason
        /// </summary>
        public virtual void Pause()
        {
            flag = TaskFlag.PAUSED;
        }

        /// <summary>
        /// Method should be invoked when worker can't execute task by himself, but task still can be done by someone else
        /// </summary>
        public virtual void Abandon()
        {
            flag = TaskFlag.ABANDONED;
            worker.Profession.Workplace.TakeTaskBackFromWorker(this);
        }

        /// <summary>
        /// Method should be invoked when there will occur any problem with continuing this task
        /// </summary>
        public virtual void Interrupt()
        {
            flag = TaskFlag.INTERRUPTED;
        }

        /// <summary>
        /// Method should be invoked when task is dependent of any other tasks that should be completed first
        /// </summary>
        public void SetWaiting()
        {
            flag = TaskFlag.WAITING;
        }

        /// <summary>
        /// Method should be invoked when task is no longer dependent on other tasks and can be executed
        /// </summary>
        public void SetReady()
        {
            flag = TaskFlag.READY;
        }

        public void Take(Villager newWorker, params Action[] taskCompleteActions)
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