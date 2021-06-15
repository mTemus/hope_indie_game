using Code.Villagers.Brain.Layers;
using UnityEngine;

namespace Code.Villagers.Brain
{
    public class Villager_Brain : MonoBehaviour
    {
        [Header("Layers")]
        [SerializeField] private Villager_Brain_PerceptionLayer perception;
        [SerializeField] private Villager_Brain_BehaviourLayer behaviour;
        [SerializeField] private Villager_Brain_MotionLayer motion;
        [SerializeField] private Villager_Brain_WorkLayer work;
        [SerializeField] private Villager_Brain_AnimationsLayer animations;
    
        public Villager_Brain_PerceptionLayer Perception => perception;
        public Villager_Brain_BehaviourLayer Behaviour => behaviour;
        public Villager_Brain_MotionLayer Motion => motion;
        public Villager_Brain_WorkLayer Work => work;
        public Villager_Brain_AnimationsLayer Animations => animations;

        private void Awake()
        {
            perception.Initialize(this);
            behaviour.Initialize(this);
            motion.Initialize(this);
            work.Initialize(this);
            animations.Initialize(this);
        }

        private void Update()
        {
            perception.ManualUpdate();
            behaviour.ManualUpdate();
        }

        public void ClearBehaviourAIComponents()
        {
            behaviour.BehaviourTree.StopBehaviour();
            behaviour.BehaviourTree.blackboard.variables.Clear();
            behaviour.BehaviourTree.graph = null;
        }
    }
}
