using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_BehaviourLayer : BrainLayer
    {
        [SerializeField] private BehaviourTreeOwner behaviourTree;

        public BehaviourTreeOwner BehaviourTree => behaviourTree;
        
        public void ManualUpdate()
        {
            BehaviourTree.Tick();
        }
    }
}
