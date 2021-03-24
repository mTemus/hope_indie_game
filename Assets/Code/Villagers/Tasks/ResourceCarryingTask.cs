using System;
using Code.Map.Building;
using Code.Map.Building.Buildings.Components;
using Code.Resources;
using Code.System.Properties;
using Code.Villagers.Entity;
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
        private readonly ProfessionType professionType;
        private readonly Vector3 storagePosition;
        private readonly Resource requiredResource;
        private ResourceCarryingTaskState resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;

        private readonly Action<Resource> onResourceDelivered;
        
        public ResourceCarryingTask(int taskPriority,
            ProfessionType professionType,
            Vector3 taskPosition, 
            Warehouse storage, 
            Resource requiredResource, 
            Action<Resource> onResourceDelivered)
        {
            base.TaskPriority = taskPriority;
            base.TaskPosition = taskPosition;
            this.professionType = professionType;
            this.storage = storage;
            this.requiredResource = requiredResource;
            this.onResourceDelivered = onResourceDelivered;

            storagePosition = storage.transform.position + storage.GetComponent<Building>().EntrancePivot;
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
                    Worker.MoveTo(storagePosition);
                    
                    if (Vector3.Distance(Worker.transform.position, storagePosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.TAKE_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.TAKE_RESOURCES:
                    Worker.Profession.CarriedResource = storage.GetResourceFromStorage(requiredResource.Type, requiredResource.amount > GlobalProperties.MAXResourceHeld ? 
                        GlobalProperties.MAXResourceHeld : requiredResource.amount);
                    Worker.UI.SetResourceIcon(true, requiredResource.Type);
                    requiredResource.amount -= Worker.Profession.CarriedResource.amount;
                    resourceCarryingState = ResourceCarryingTaskState.GO_ON_TASK_POSITION;
                    break;
                
                case ResourceCarryingTaskState.GO_ON_TASK_POSITION:
                    Worker.MoveTo(TaskPosition);
                    if (Vector3.Distance(Worker.transform.position, TaskPosition) <= 0.1f)
                        resourceCarryingState = ResourceCarryingTaskState.DELIVER_RESOURCES;
                    break;
                
                case ResourceCarryingTaskState.DELIVER_RESOURCES:
                    onResourceDelivered?.Invoke(Worker.Profession.CarriedResource);
                    Worker.UI.SetResourceIcon(false, requiredResource.Type);

                    if (requiredResource.amount != 0) 
                        resourceCarryingState = ResourceCarryingTaskState.GO_TO_STORAGE;
                    else {
                        OnTaskCompleted.Invoke();
                    }
                    break;
            }
        }

        public override void OnTaskPause()
        {
            
        }

        public void SetWorker(Villager newWorker)
        {
            Worker = newWorker;
        }

        public ProfessionType ProfessionType => professionType;
    }
}
