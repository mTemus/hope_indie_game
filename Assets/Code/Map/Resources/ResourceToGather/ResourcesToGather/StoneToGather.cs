using Code.Map.Building;
using Code.Villagers.Entity;
using NotImplementedException = System.NotImplementedException;

namespace Code.Map.Resources.ResourceToGather.ResourcesToGather
{
    public class StoneToGather : ResourceToGather
    {
        private Workplace workplace;

        public override void OnGatherStart(Villager worker)
        {
            throw new NotImplementedException();
        }

        public override bool Gather(Villager worker, int socketId)
        {
            throw new NotImplementedException();
        }

        protected override void OnResourceDepleted()
        {
            throw new NotImplementedException();
        }
    }
}
