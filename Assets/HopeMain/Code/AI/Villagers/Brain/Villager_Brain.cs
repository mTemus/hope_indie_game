using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public class Villager_Brain : EntityBrain
    {
        [Header("Layers")]
        [SerializeField] private Villager_Brain_PerceptionLayer perception;
        [SerializeField] private Villager_Brain_BehaviourLayer behaviour;
        [SerializeField] private Villager_Brain_MotionLayer motion;
        [SerializeField] private Villager_Brain_WorkLayer work;
        [SerializeField] private Villager_Brain_AnimationsLayer animations;
        [SerializeField] private Villager_Brain_SoundsLayer sounds;
    
        public Villager_Brain_PerceptionLayer Perception => perception;
        public Villager_Brain_BehaviourLayer Behaviour => behaviour;
        public Villager_Brain_MotionLayer Motion => motion;
        public Villager_Brain_WorkLayer Work => work;
        public Villager_Brain_AnimationsLayer Animations => animations;
        public Villager_Brain_SoundsLayer Sounds => sounds;

        private void Awake()
        {
            onWalkingSoundSet = sounds.SetWalkingAudioClip;

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
