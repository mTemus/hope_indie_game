using HopeMain.Code.Characters.Villagers.Entity;

namespace HopeMain.Code.World.Resources.ResourceToGather
{
    /// <summary>
    /// 
    /// </summary>
    public class Wood : ResourceToGatherBase
    {
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
            foreach (Villager gatherer in gatherers.Keys) 
                gatherers[gatherer].DepleteCurrentResource();

            foreach (Villager gatherer in gatherers.Keys) 
                UnregisterGatherer(gatherer);

            StartCoroutine(ClearResource());
        }
    }
}
