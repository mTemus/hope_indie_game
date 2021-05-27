using NodeCanvas.BehaviourTrees;
using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_BehaviourLayer : MonoBehaviour, IBrainLayer
    {
        public BehaviourTreeOwner BehaviourTree { get; set; }

        
        public void Initialize(Villager_Brain brain)
        {
            
        }
    }
}
