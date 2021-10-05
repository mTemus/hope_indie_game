using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.System;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Resources;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public class ResourceGathering_Single : ResourceGathering
    {
        public ResourceGathering_Single(ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentGatheringState = ResourceGatheringFlag.FIND_CLOSEST_RESOURCE;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.RUNNING;
            
            switch (currentGatheringState) {
                case ResourceGatheringFlag.GO_TO_WORKPLACE:
                    if (!worker.Brain.Motion.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    currentGatheringState = ResourceGatheringFlag.DELIVER_RESOURCE_TO_WORKPLACE;
                    break;
                
                case ResourceGatheringFlag.FIND_CLOSEST_RESOURCE:
                    Vector3 currWorkerPosition = worker.transform.position;
                    
                    Area resourceArea =
                        Managers.I.Areas.FindClosestAreaOfTypes(currWorkerPosition, gatherAreas);
                    resourceToGather =
                        resourceArea.GetClosestResourceToGatherByType(currWorkerPosition, resourceType);

                    if (resourceToGather == null) {
                        if (worker.Profession.IsCarryingResource) {
                            currentGatheringState = ResourceGatheringFlag.GO_TO_WORKPLACE;
                        }
                        else {
                            worker.Profession.CarriedResource = null;
                            worker.Brain.Work.AbandonCurrentTask();
                            return;
                        }
                    }
                    
                    gatheringSocketId = resourceToGather.RegisterGatherer(worker, this);
                    resourcePosition = resourceToGather.PivotedPosition;
                    currentGatheringState = ResourceGatheringFlag.GO_TO_RESOURCE;
                    break;
                
                case ResourceGatheringFlag.GO_TO_RESOURCE:
                    if (!worker.Brain.Motion.MoveTo(resourcePosition)) break;
                    resourceToGather.StartGathering(worker);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    currentGatheringState = ResourceGatheringFlag.GATHER_RESOURCE;
                    break;
                
                case ResourceGatheringFlag.GATHER_RESOURCE:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                    worker.UI.SetResourceIcon(worker.Profession.CarriedResource.Type);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
                    currentGatheringState = ResourceGatheringFlag.GO_TO_WORKPLACE;
                    break;
                
                case ResourceGatheringFlag.DELIVER_RESOURCE_TO_WORKPLACE:
                    onResourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    worker.Profession.CarriedResource = null;
                    
                    currentGatheringState = resourceToGather != null ? 
                        ResourceGatheringFlag.GO_TO_RESOURCE : ResourceGatheringFlag.FIND_CLOSEST_RESOURCE;
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
            if (currentGatheringState != ResourceGatheringFlag.GO_TO_WORKPLACE) 
                currentGatheringState = ResourceGatheringFlag.FIND_CLOSEST_RESOURCE;

            resourceToGather = null; 
        }
        
    }
}
