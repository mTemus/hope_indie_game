using Code.Map.Building.Workplaces;
using Code.Villagers.Entity;
using NotImplementedException = System.NotImplementedException;

namespace Code.Map.Resources.ResourceToGather.ResourcesToGather
{
    public class StoneToGather : ResourceToGather
    {
        private Workplace workplace;

        public Workplace Workplace
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
