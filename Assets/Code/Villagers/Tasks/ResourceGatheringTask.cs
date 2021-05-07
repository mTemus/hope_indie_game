using System;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.System;
using Code.System.Areas;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum ResourceGatheringTaskState
    {
        GO_TO_WORKPLACE,
        FIND_CLOSEST_RESOURCE,
        GO_TO_RESOURCE,
        GATHER_RESOURCE,
        DELIVER_RESOURCE_TO_WORKPLACE
    }
    
    public class ResourceGatheringTask : Task
    {
        private readonly ResourceType resourceType;
        private readonly AreaType[] gatherAreas;
        
        private ResourceGatheringTaskState currentGatheringState;
        private ResourceToGather resourceToGather;
        private Vector3 resourcePosition;
        
        private int gatheringSocketId;

        public Action<Resource> onResourceDelivery;
        
        public ResourceGatheringTask(ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public override void StartTask()
        {
            currentGatheringState = ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
        }
        
        public override void DoTask()
        {
            switch (currentGatheringState) {
                case ResourceGatheringTaskState.GO_TO_WORKPLACE:
                    if (!worker.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    currentGatheringState = worker.Profession.IsCarryingResource ? 
                        ResourceGatheringTaskState.DELIVER_RESOURCE_TO_WORKPLACE : ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
                    break;
                
                case ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE:
                    Vector3 currWorkerPosition = worker.transform.position;
                    
                    Area resourceArea =
                        Managers.I.Areas.FindClosestAreaOfTypes(currWorkerPosition, gatherAreas);
                    resourceToGather =
                        resourceArea.GetClosestResourceToGatherByType(currWorkerPosition, resourceType);

                    if (resourceToGather == null) {
                        if (worker.Profession.IsCarryingResource) {
                            currentGatheringState = ResourceGatheringTaskState.GO_TO_WORKPLACE;
                        }
                        else {
                            worker.Profession.CarriedResource = null;
                            worker.Profession.AbandonCurrentTask();
                            return;
                        }
                    }
                    
                    gatheringSocketId = resourceToGather.RegisterGatherer(worker, this);
                    resourcePosition = resourceToGather.PivotedPosition;
                    currentGatheringState = ResourceGatheringTaskState.GO_TO_RESOURCE;
                    break;
                
                case ResourceGatheringTaskState.GO_TO_RESOURCE:
                    if (!worker.MoveTo(resourcePosition)) break;
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = ResourceGatheringTaskState.GATHER_RESOURCE;
                    break;
                
                case ResourceGatheringTaskState.GATHER_RESOURCE:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                        worker.UI.SetResourceIcon(true, worker.Profession.CarriedResource.Type);
                        currentGatheringState = ResourceGatheringTaskState.GO_TO_WORKPLACE;
                    break;
                
                case ResourceGatheringTaskState.DELIVER_RESOURCE_TO_WORKPLACE:
                    onResourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false, worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;
                    
                    currentGatheringState = resourceToGather != null ? 
                        ResourceGatheringTaskState.GO_TO_RESOURCE : ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void EndTask() {}
        public override void PauseTask() {}
        public override void AbandonTask() {}

        public void DepleteCurrentResource()
        {
            if (currentGatheringState != ResourceGatheringTaskState.GO_TO_WORKPLACE) 
                currentGatheringState = ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;

            resourceToGather = null; 
        }
    }
}
