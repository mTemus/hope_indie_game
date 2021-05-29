using System;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.System;
using Code.System.Areas;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public enum Task_ResourceGathering_State
    {
        GO_TO_WORKPLACE,
        FIND_CLOSEST_RESOURCE,
        GO_TO_RESOURCE,
        GATHER_RESOURCE,
        DELIVER_RESOURCE_TO_WORKPLACE
    }
    
    public class Task_ResourceGathering : Task
    {
        private readonly ResourceType resourceType;
        private readonly AreaType[] gatherAreas;
        
        private Task_ResourceGathering_State currentGatheringState;
        private ResourceToGather resourceToGather;
        private Vector3 resourcePosition;
        
        private int gatheringSocketId;

        public Action<Resource> onResourceDelivery;
        
        public Task_ResourceGathering(ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public override void Start()
        {
            currentGatheringState = Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;
        }
        
        public override void Execute()
        {
            state = TaskState.RUNNING;
            
            switch (currentGatheringState) {
                case Task_ResourceGathering_State.GO_TO_WORKPLACE:
                    if (!worker.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    currentGatheringState = worker.Profession.IsCarryingResource ? 
                        Task_ResourceGathering_State.DELIVER_RESOURCE_TO_WORKPLACE : Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;
                    break;
                
                case Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE:
                    Vector3 currWorkerPosition = worker.transform.position;
                    
                    Area resourceArea =
                        Managers.I.Areas.FindClosestAreaOfTypes(currWorkerPosition, gatherAreas);
                    resourceToGather =
                        resourceArea.GetClosestResourceToGatherByType(currWorkerPosition, resourceType);

                    if (resourceToGather == null) {
                        if (worker.Profession.IsCarryingResource) {
                            currentGatheringState = Task_ResourceGathering_State.GO_TO_WORKPLACE;
                        }
                        else {
                            worker.Profession.CarriedResource = null;
                            worker.Brain.Work.AbandonCurrentTask();
                            return;
                        }
                    }
                    
                    gatheringSocketId = resourceToGather.RegisterGatherer(worker, this);
                    resourcePosition = resourceToGather.PivotedPosition;
                    currentGatheringState = Task_ResourceGathering_State.GO_TO_RESOURCE;
                    break;
                
                case Task_ResourceGathering_State.GO_TO_RESOURCE:
                    if (!worker.MoveTo(resourcePosition)) break;
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = Task_ResourceGathering_State.GATHER_RESOURCE;
                    break;
                
                case Task_ResourceGathering_State.GATHER_RESOURCE:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                        worker.UI.SetResourceIcon(worker.Profession.CarriedResource.Type);
                        currentGatheringState = Task_ResourceGathering_State.GO_TO_WORKPLACE;
                    break;
                
                case Task_ResourceGathering_State.DELIVER_RESOURCE_TO_WORKPLACE:
                    onResourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    worker.Profession.CarriedResource = null;
                    
                    currentGatheringState = resourceToGather != null ? 
                        Task_ResourceGathering_State.GO_TO_RESOURCE : Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void End() {}
        public override void Pause() {}

        public void DepleteCurrentResource()
        {
            if (currentGatheringState != Task_ResourceGathering_State.GO_TO_WORKPLACE) 
                currentGatheringState = Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;

            resourceToGather = null; 
        }
    }
}
