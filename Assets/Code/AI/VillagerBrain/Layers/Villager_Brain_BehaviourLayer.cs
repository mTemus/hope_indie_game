using NodeCanvas.BehaviourTrees;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_BehaviourLayer : BrainLayer
    {
        public BehaviourTreeOwner BehaviourTree { get; private set; }

        private void Awake()
        {
            BehaviourTree = GetComponent<BehaviourTreeOwner>();
        }
    }
}
