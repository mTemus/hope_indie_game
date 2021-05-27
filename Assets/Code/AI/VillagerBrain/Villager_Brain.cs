using Code.AI.VillagerBrain.Layers;
using UnityEngine;

namespace Code.AI.VillagerBrain
{
    public class Villager_Brain : MonoBehaviour
    {
        [Header("Layers")]
        [SerializeField] private Villager_Brain_PerceptionLayer perception;
        [SerializeField] private Villager_Brain_BehaviourLayer behaviour;
        [SerializeField] private Villager_Brain_MotionLayer motion;
    
        public Villager_Brain_PerceptionLayer Perception => perception;
        public Villager_Brain_BehaviourLayer Behaviour => behaviour;
        public Villager_Brain_MotionLayer Motion => motion;

        private void Awake()
        {
            perception.Initialize(this);
            behaviour.Initialize(this);
            motion.Initialize(this);
        }
    }
}
