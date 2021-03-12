using System;
using Code.Map.Building.Buildings.Components;
using Code.Resources;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum ResourceCarryingTaskState
    {
        GO_TO_STORAGE, 
        TAKE_RESOURCES,
        GO_ON_TASK_POSITION,
        DELIVER_RESOURCES
    }

    public class ResourceCarryingTask : Task
    {
        private readonly Warehouse storage;
        private Resource resourceData;
        private ResourceCarryingTaskState resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;

        private readonly Func<Resource, Resource> onResourceDelivered;
        
        public ResourceCarryingTask(int taskPriority, 
            Vector3 taskPosition, 
            Warehouse storage, 
            Resource resourceData, 
            Func<Resource, Resource> onResourceDelivered)
        {
            base.TaskPriority = taskPriority;
            base.TaskPosition = taskPosition;
            this.storage = storage;
            this.resourceData = resourceData;
            this.onResourceDelivered = onResourceDelivered;
        }

        public override void OnTaskStart()
        {
           
        }

        public override void OnTaskEnd()
        {
            
        }

        public override void DoTask()
        {
            switch (resourceCarryingState) {
                case ResourceCarryingTaskState.GO_TO_STORAGE:
                    Worker.MoveTo(TaskPosition);

                    if (Vector3.Distance(Worker.transform.position, storage.transform.position) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.TAKE_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.TAKE_RESOURCES:
                    Worker.GetComponent<Profession>().CarriedResource = storage.GetResource(resourceData.Type, resourceData.Amount);
                    resourceCarryingState = ResourceCarryingTaskState.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingTaskState.GO_ON_TASK_POSITION:
                    Worker.MoveTo(TaskPosition);
                    if (Vector3.Distance(Worker.transform.position, TaskPosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.DELIVER_RESOURCES:
                    resourceData = onResourceDelivered?.Invoke(Worker.GetComponent<Profession>().CarriedResource);

                    if (resourceData != null && resourceData.Amount != 0) 
                        resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    else 
                        OnTaskCompleted?.Invoke();
                    break;
            }
            
            
            // if (Worker.CarriedResource == null) {
            //     if (Vector3.Distance(workerTransform.position, storage.transform.position) <= 0.1f) 
            //         Worker.MoveTo(TaskPosition);
            //     else 
            //         Worker.CarriedResource = storage.GetResource(resourceData.Type, resourceData.Amount);
            // }
            // else {
            //     if (Vector3.Distance(workerTransform.position, TaskPosition) <= 0.1f) {
            //         Worker.MoveTo(TaskPosition);
            //     }
            //     else {
            //         resourceData = onResourceDelivered.Invoke(Worker.CarriedResource);
            //         Worker.CarriedResource = null;
            //
            //         if (resourceData.Amount == 0) 
            //             OnTaskComplete.Invoke();
            //     }
            // }
        }

        public void SetWorker(Villager newWorker)
        {
            Worker = newWorker;
        }

        public int TaskPriority => base.TaskPriority;
    }
}
