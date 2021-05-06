using System;
using Code.Map.Building.Buildings.Modules;
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
        private ResourceGatheringTaskState currentGatheringState = ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
        private ResourceToGather resourceToGather;
        private readonly BuildingStorageModule storage;
        private readonly ResourceType resourceType;
        private readonly AreaType[] gatherAreas;
        private int gatheringSocketId;
        private Vector3 resourcePosition;
        
        public ResourceGatheringTask(BuildingStorageModule storage, ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.storage = storage;
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public ResourceGatheringTask(ResourceToGather resourceToGather, BuildingStorageModule storage, ResourceType resourceType, AreaType[] gatherAreas) 
            :this(storage, resourceType, gatherAreas)
        {
            this.resourceToGather = resourceToGather;
            currentGatheringState = ResourceGatheringTaskState.GO_TO_RESOURCE;
        }

        public override void StartTask()
        {
        }

        public override void EndTask()
        {
        }

        public override void DoTask()
        {
            switch (currentGatheringState) {
                case ResourceGatheringTaskState.GO_TO_WORKPLACE:
                    worker.MoveTo(worker.Profession.Workplace.PivotedPosition);
                    
                    if (Vector3.Distance(worker.transform.position, worker.Profession.Workplace.PivotedPosition) >= 0.1f) break;
                    currentGatheringState = worker.Profession.CarriedResource != null ? 
                        ResourceGatheringTaskState.DELIVER_RESOURCE_TO_WORKPLACE : ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
                    break;
                
                case ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE:
                    Vector3 currWorkerPosition = worker.transform.position;
                    
                    Area resourceArea =
                        Managers.I.Areas.FindClosestAreaOfTypes(currWorkerPosition, gatherAreas);
                    resourceToGather =
                        resourceArea.GetClosestResourceToGatherByType(currWorkerPosition, resourceType);

                    if (resourceToGather == null) {
                        if (worker.Profession.CarriedResource != null && worker.Profession.CarriedResource.amount > 0) {
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
                    worker.MoveTo(resourcePosition);
                    
                    if (Vector3.Distance(worker.transform.position, resourcePosition) >= 0.1f) break;
                    resourceToGather.OnGatherStart(worker);
                    currentGatheringState = ResourceGatheringTaskState.GATHER_RESOURCE;
                    break;
                
                case ResourceGatheringTaskState.GATHER_RESOURCE:
                    if (!resourceToGather.Gather(worker, gatheringSocketId)) {
                        worker.UI.SetResourceIcon(true, worker.Profession.CarriedResource.Type);
                        currentGatheringState = ResourceGatheringTaskState.GO_TO_WORKPLACE;
                    }
                    break;
                
                case ResourceGatheringTaskState.DELIVER_RESOURCE_TO_WORKPLACE:
                    
                    storage.StoreResource(worker.Profession.CarriedResource);
                    worker.UI.SetResourceIcon(false, worker.Profession.CarriedResource.Type);
                    worker.Profession.CarriedResource = null;
                    
                    currentGatheringState = resourceToGather != null ? 
                        ResourceGatheringTaskState.GO_TO_RESOURCE : ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void PauseTask()
        {
        }

        public override void AbandonTask()
        {
        }

        public void OnCurrentResourceDepleted()
        {
            if (currentGatheringState != ResourceGatheringTaskState.GO_TO_WORKPLACE) 
                currentGatheringState = ResourceGatheringTaskState.FIND_CLOSEST_RESOURCE;

            resourceToGather = null; 
        }
    }
}
