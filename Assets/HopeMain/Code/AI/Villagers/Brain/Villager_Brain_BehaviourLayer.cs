using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public class Villager_Brain_BehaviourLayer : Villager_BrainLayer
    {
        [SerializeField] private BehaviourTreeOwner behaviourTree;

        public BehaviourTreeOwner BehaviourTree => behaviourTree;
        
        public override void Initialize(Villager_Brain villagerBrain) {}
        
        public void ManualUpdate()
        {
            BehaviourTree.Tick();
        }

        
    }
}
