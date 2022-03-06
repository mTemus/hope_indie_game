using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace _Prototype.Code.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public class BehaviourLayer : BrainLayer
    {
        [SerializeField] private BehaviourTreeOwner behaviourTree;

        public BehaviourTreeOwner BehaviourTree => behaviourTree;
        
        public override void Initialize(Brain brain) {}
        
        /// <summary>
        /// 
        /// </summary>
        public void ManualUpdate()
        {
            BehaviourTree.Tick();
        }

        
    }
}
