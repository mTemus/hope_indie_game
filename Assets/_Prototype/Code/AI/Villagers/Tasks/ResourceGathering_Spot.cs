using System;
using _Prototype.Code.AI.Villagers.Brain;
using _Prototype.Code.World.Resources.ResourceToGather;

namespace _Prototype.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceGatheringSpot : ResourceGathering
    {
        public ResourceGatheringSpot(ResourceToGatherBase resourceToGather)
        {
            this.resourceToGather = resourceToGather;
        }

        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentGatheringState = ResourceGatheringFlag.GOToWorkplace;
        }

        public override void End()
        {
            flag = TaskFlag.Completed;
        }

        public override void Execute()
        {
            flag = TaskFlag.Running;

            switch (currentGatheringState) {
                case ResourceGatheringFlag.GOToWorkplace:
                    if (!worker.Brain.Motion.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    worker.Profession.Workplace.WorkerEntersWorkplace(worker);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = ResourceGatheringFlag.GatherResource;
                    break;

                case ResourceGatheringFlag.GatherResource:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                    currentGatheringState = ResourceGatheringFlag.DeliverResourceToWorkplace;
                    break;
                
                case ResourceGatheringFlag.DeliverResourceToWorkplace:
                    resourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.Profession.CarriedResource = null;
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = ResourceGatheringFlag.GatherResource;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void DepleteCurrentResource()
        {
        }
    }
}
