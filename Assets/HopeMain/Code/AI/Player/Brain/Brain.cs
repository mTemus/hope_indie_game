using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System;
using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    public class Brain : EntityBrain
    {
        [Header("Player components")] 
        [SerializeField] private MotionLayer motion;
        [SerializeField] private AnimationsLayer animations;
        [SerializeField] private SoundsLayer sounds;
        
        public MotionLayer Motion => motion;
        public AnimationsLayer Animations => animations;
        public SoundsLayer Sounds => sounds;

        private void Awake()
        {
            onWalkingSoundSet = sounds.SetWalkingAudioClip;
        
            motion.Initialize(this);
            animations.Initialize(this);
            sounds.Initialize(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.I.Selection.AddVillagerToSelect(other.gameObject.GetComponent<Villager>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.I.Selection.RemoveVillagerToSelect(other.GetComponent<Villager>());
            }
        }
    }
}
