using System;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System.Assets;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum TaskFlag
    {
        New,
        Waiting,
        Ready,
        Running,
        Interrupted,
        Paused,
        Abandoned,
        Completed
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Task
    {
        protected Vector3 taskPosition;
        protected Villager worker;

        public TaskFlag flag = TaskFlag.New;
        
        public Action taskCompleted;
        public Action taskCancel;
        public Action taskSetReady;
        
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
            flag = TaskFlag.Paused;
        }

        /// <summary>
        /// Method should be invoked when worker can't execute task by himself, but task still can be done by someone else
        /// </summary>
        public virtual void Abandon()
        {
            flag = TaskFlag.Abandoned;
            worker.Profession.Workplace.TakeTaskBackFromWorker(this);
        }

        /// <summary>
        /// Method should be invoked when there will occur any problem with continuing this task
        /// </summary>
        public virtual void Interrupt()
        {
            flag = TaskFlag.Interrupted;
        }

        /// <summary>
        /// Method should be invoked when task is dependent of any other tasks that should be completed first
        /// </summary>
        public void SetWaiting()
        {
            flag = TaskFlag.Waiting;
        }

        /// <summary>
        /// Method should be invoked when task is no longer dependent on other tasks and can be executed
        /// </summary>
        public void SetReady()
        {
            flag = TaskFlag.Ready;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newWorker"></param>
        /// <param name="taskCompleteActions"></param>
        public void Take(Villager newWorker, params Action[] taskCompleteActions)
        {
            worker = newWorker;
        
            foreach (Action taskCompleted in taskCompleteActions) 
                this.taskCompleted += taskCompleted;
        }

        protected void ThrowResourceOnGround()
        {
            worker.UI.ClearResourceIcon();
            AssetsStorage.I.ThrowResourceOnTheGround(worker.Profession.CarriedResource, worker.transform.position.x);
            worker.Profession.CarriedResource = null;
        }
    }
}