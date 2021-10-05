using System;
using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.World.Buildings.Workplace;

namespace HopeMain.Code.World.Resources.ResourceToGather
{
    public class Stone : ResourceToGatherBase
    {
        private WorkplaceBase workplace;

        public WorkplaceBase Workplace
        {
            get => workplace;
            set => workplace = value;
        }

        public bool hasWorkplace => workplace != null;

        public override void StartGathering(Villager worker)
        {
            if (worker.Profession.IsCarryingResource) return;
            worker.Profession.CarriedResource = new Resource(resource.Type, 0);
        }

        public override bool Gather(Villager worker, int socketId)
        {
            float gatheringFormula = 1f + 0.1f * worker.Statistics.Strength + 0.6f * worker.Statistics.Dexterity;
            gatheringSockets[socketId].GatherResource(gatheringFormula, worker.Profession);

            return worker.Profession.CarriedResource.amount != worker.Profession.Data.ResourceCarryingLimit;
        }

        protected override void DepleteResource()
        {
            throw new NotImplementedException();
        }
    }
}
