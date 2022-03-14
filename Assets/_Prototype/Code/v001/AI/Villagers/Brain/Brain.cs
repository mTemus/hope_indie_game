using UnityEngine;

namespace _Prototype.Code.v001.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public class Brain : EntityBrain
    {
        [Header("Layers")]
        [SerializeField] private PerceptionLayer perception;
        [SerializeField] private BehaviourLayer behaviour;
        [SerializeField] private MotionLayer motion;
        [SerializeField] private WorkLayer work;
        [SerializeField] private AnimationsLayer animations;
        [SerializeField] private SoundsLayer sounds;
    
        public PerceptionLayer Perception => perception;
        public BehaviourLayer Behaviour => behaviour;
        public MotionLayer Motion => motion;
        public WorkLayer Work => work;
        public AnimationsLayer Animations => animations;
        public SoundsLayer Sounds => sounds;

        private void Awake()
        {
            walkingSoundSet = sounds.SetWalkingAudioClip;

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

        /// <summary>
        /// 
        /// </summary>
        public void ClearBehaviourAIComponents()
        {
            behaviour.BehaviourTree.StopBehaviour();
            behaviour.BehaviourTree.blackboard.variables.Clear();
            behaviour.BehaviourTree.graph = null;
        }
    }
}
