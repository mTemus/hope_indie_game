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
        [SerializeField] private Villager_Brain_WorkLayer work;
    
        public Villager_Brain_PerceptionLayer Perception => perception;
        public Villager_Brain_BehaviourLayer Behaviour => behaviour;
        public Villager_Brain_MotionLayer Motion => motion;
        public Villager_Brain_WorkLayer Work => work;

        private void Awake()
        {
            perception.Initialize(this);
            behaviour.Initialize(this);
            motion.Initialize(this);
        }

        private void Update()
        {
            perception.Update();
        }

        public void ClearBehaviourAIComponents()
        {
            behaviour.BehaviourTree.StopBehaviour();
            behaviour.BehaviourTree.blackboard.variables.Clear();
            behaviour.BehaviourTree.graph = null;
        }
    }
}
