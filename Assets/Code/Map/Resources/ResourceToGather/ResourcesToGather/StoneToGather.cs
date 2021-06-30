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
            throw new NotImplementedException();
        }

        public override bool Gather(Villager worker, int socketId)
        {
            throw new NotImplementedException();
        }

        protected override void DepleteResource()
        {
            throw new NotImplementedException();
        }
    }
}
