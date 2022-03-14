using System;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.World.Buildings.Workplaces;

namespace _Prototype.Code.v001.World.Resources.ResourceToGather
{
    /// <summary>
    /// 
    /// </summary>
    public class Stone : ResourceToGatherBase
    {
        private Workplace _workplace;

        public Workplace Workplace
        {
            get => _workplace;
            set => _workplace = value;
        }

        public bool HasWorkplace => _workplace != null;

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
