using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public class BehaviourLayer : BrainLayer
    {
        [SerializeField] private BehaviourTreeOwner behaviourTree;

        public BehaviourTreeOwner BehaviourTree => behaviourTree;
        
        public override void Initialize(Brain brain) {}
        
        public void ManualUpdate()
        {
            BehaviourTree.Tick();
        }

        
    }
}
