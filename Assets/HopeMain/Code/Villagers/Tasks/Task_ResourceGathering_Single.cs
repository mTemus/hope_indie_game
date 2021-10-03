using System;
using Code.Map.Resources;
using Code.System;
using Code.System.Areas;
using Code.Villagers.Brain.Layers;
using UnityEngine;

namespace Code.Villagers.Tasks
{
    public class Task_ResourceGathering_Single : Task_ResourceGathering
    {
        public Task_ResourceGathering_Single(ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentGatheringState = Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;
            
            switch (currentGatheringState) {
                case Task_ResourceGathering_State.GO_TO_WORKPLACE:
                    if (!worker.Brain.Motion.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    currentGatheringState = Task_ResourceGathering_State.DELIVER_RESOURCE_TO_WORKPLACE;
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
                    if (!worker.Brain.Motion.MoveTo(resourcePosition)) break;
                    resourceToGather.StartGathering(worker);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    currentGatheringState = Task_ResourceGathering_State.GATHER_RESOURCE;
                    break;
                
                case Task_ResourceGathering_State.GATHER_RESOURCE:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                    worker.UI.SetResourceIcon(worker.Profession.CarriedResource.Type);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
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

        public override void End()
        {
            flag = TaskFlag.COMPLETED;
        }
        
        public override void DepleteCurrentResource()
        {
            if (currentGatheringState != Task_ResourceGathering_State.GO_TO_WORKPLACE) 
                currentGatheringState = Task_ResourceGathering_State.FIND_CLOSEST_RESOURCE;

            resourceToGather = null; 
        }
        
    }
}
