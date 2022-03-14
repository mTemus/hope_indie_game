using System.Collections.Generic;
using _Prototype.Code.v001.AI.Villagers.Tasks;
using _Prototype.Code.v001.System.Assets;
using _Prototype.Code.v001.World.Resources;

namespace _Prototype.Code.v001.AI.Villagers.Brain
{
    public class WorkLayer : BrainLayer
    {
        private readonly Queue<Task> _tasks = new Queue<Task>();
        
        private Task _currentTask;
        
        public Resource CarriedResource { get; set; }
        public bool HasWorkToDo => _tasks.Count > 0 || _currentTask != null || _currentTask != null && _currentTask.flag != TaskFlag.Completed;
        public bool TaskComplete => _currentTask.flag == TaskFlag.Completed;
        public bool IsCarryingResource => CarriedResource != null && CarriedResource.amount > 0;

        public override void Initialize(Brain brain) {}
        
        /// <summary>
        /// Called in Worker Behaviour Tree
        /// </summary>
        public void Work()
        { 
            _currentTask.Execute();
        }
       
        #region Tasks
        
        private void AbandonTask(Task task)
        {
            task.Abandon();
        }
        
        /// <summary>
        /// Called in Worker Behaviour Tree
        /// </summary>
        /// <returns></returns>
        public bool GetNewTask()
        {
            if (_tasks.Count <= 0) {
                _currentTask = null;
                return false;
            }
            
            _currentTask = _tasks.Dequeue();
            _currentTask.Start();
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            _tasks.Enqueue(task);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PauseCurrentTask()
        {
            _currentTask.Pause();
            AddTask(_currentTask);
            _currentTask = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AbandonCurrentTask()
        {
            if (_currentTask != null) {
                AbandonTask(_currentTask);
                _currentTask = null;
            }

            if (!IsCarryingResource) return;
            AssetsStorage.I.ThrowResourceOnTheGround(CarriedResource, transform.position.x);
            CarriedResource = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void AbandonAllTasks()
        {
            if (!HasWorkToDo) 
                return;

            AbandonTask(_currentTask);
            
            foreach (Task task in _tasks) 
                AbandonTask(task);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InterruptTask()
        {
            _currentTask.Interrupt();
            AddTask(_currentTask);
            _currentTask = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CompleteTask()
        {
            _currentTask.End();
        }
        
        #endregion
    }
}
