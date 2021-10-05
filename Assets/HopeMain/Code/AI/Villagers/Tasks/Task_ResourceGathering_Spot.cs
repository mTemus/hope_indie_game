using System;
using HopeMain.Code.AI.Villagers.Brain;
using HopeMain.Code.World.Resources.ResourceToGather;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public class Task_ResourceGathering_Spot : Task_ResourceGathering
    {
        // Start is called before the first frame update
        public Task_ResourceGathering_Spot(ResourceToGather resourceToGather)
        {
            this.resourceToGather = resourceToGather;
        }

        public override void Start()
        {
            worker.Brain.Animations.SetState(VillagerAnimationState.Walk);
            currentGatheringState = Task_ResourceGathering_State.GO_TO_WORKPLACE;
        }

        public override void End()
        {
            flag = TaskFlag.COMPLETED;
        }

        public override void Execute()
        {
            flag = TaskFlag.RUNNING;

            switch (currentGatheringState) {
                case Task_ResourceGathering_State.GO_TO_WORKPLACE:
                    if (!worker.Brain.Motion.MoveTo(worker.Profession.Workplace.PivotedPosition)) break;
                    worker.Profession.Workplace.WorkerEntersWorkplace(worker);
                    worker.Brain.Animations.SetState(VillagerAnimationState.Idle);
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = Task_ResourceGathering_State.GATHER_RESOURCE;
                    break;

                case Task_ResourceGathering_State.GATHER_RESOURCE:
                    if (resourceToGather.Gather(worker, gatheringSocketId)) break;
                    currentGatheringState = Task_ResourceGathering_State.DELIVER_RESOURCE_TO_WORKPLACE;
                    break;
                
                case Task_ResourceGathering_State.DELIVER_RESOURCE_TO_WORKPLACE:
                    onResourceDelivery.Invoke(worker.Profession.CarriedResource);
                    worker.Profession.CarriedResource = null;
                    resourceToGather.StartGathering(worker);
                    currentGatheringState = Task_ResourceGathering_State.GATHER_RESOURCE;
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
