using System;
using _Prototype.Code.AI.Villagers.Brain;
using _Prototype.Code.System;
using _Prototype.Code.World.Areas;
using _Prototype.Code.World.Resources;
using UnityEngine;

namespace _Prototype.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceGatheringSingle : ResourceGathering
    {
        public ResourceGatheringSingle(ResourceType resourceType, AreaType[] gatherAreas)
        {
            this.resourceType = resourceType;
            this.gatherAreas = gatherAreas;
        }
        
        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentGatheringState = ResourceGatheringFlag.FindClosestResource;
        }
        
        public override void Execute()
        {
            flag = TaskFlag.Running;
            
            switch (currentGatheringState) {
                case ResourceGatheringFlag.GOToWorkplace:
                    if (!worker.Brain.Motion.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    currentGatheringState = ResourceGatheringFlag.DeliverResourceToWorkplace;
                    break;
                
                case ResourceGatheringFlag.FindClosestResource:
                    Vector3 currWorkerPosition = worker.transform.position;
                    
                    Area resourceArea =
                        Managers.I.Areas.FindClosestAreaOfTypes(currWorkerPosition, gatherAreas);
                    resourceToGather =
                        resourceArea.GetClosestResourceToGatherByType(currWorkerPosition, resourceType);

                    if (resourceToGather == null) {
                        if (worker.Profession.IsCarryingResource) {
                            currentGatheringState = ResourceGatheringFlag.GOToWorkplace;
                        }
                        else {
                            worker.Profession.CarriedResource = null;
                            worker.Brain.Work.AbandonCurrentTask();
                            return;
                        }
                    }
                    
                    gatheringSocketId = resourceToGather.RegisterGatherer(worker, this);
                    resourcePosition = resourceToGather.PivotedPosition;
                    currentGatheringState = ResourceGatheringFlag.GOToResource;
                    break;
                
                case ResourceGatheringFlag.GOToResource:
                    if (!worker.Brain.Motion.MoveTo(resourcePosition)) break;
                    resourceToGather.StartGathering(worker);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    currentGatheringState = ResourceGatheringFlag.GatherResource;
                    break;
                
                case ResourceGatheringFlag.GatherResource:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                    worker.UI.SetResourceIcon(worker.Profession.CarriedResource.Type);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
                    currentGatheringState = ResourceGatheringFlag.GOToWorkplace;
                    break;
                
                case ResourceGatheringFlag.DeliverResourceToWorkplace:
                    resourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.UI.ClearResourceIcon();
                    worker.Profession.CarriedResource = null;
                    
                    currentGatheringState = resourceToGather != null ? 
                        ResourceGatheringFlag.GOToResource : ResourceGatheringFlag.FindClosestResource;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void End()
        {
            flag = TaskFlag.Completed;
        }
        
        public override void DepleteCurrentResource()
        {
            if (currentGatheringState != ResourceGatheringFlag.GOToWorkplace) 
                currentGatheringState = ResourceGatheringFlag.FindClosestResource;

            resourceToGather = null; 
        }
        
    }
}
