using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.System;
using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    public class Player_Brain : EntityBrain
    {
        [Header("Player components")] 
        [SerializeField] private Player_Brain_MotionLayer motion;
        [SerializeField] private Player_Brain_AnimationsLayer animations;
        [SerializeField] private Player_Brain_SoundsLayer sounds;
        
        public Player_Brain_MotionLayer Motion => motion;
        public Player_Brain_AnimationsLayer Animations => animations;
        public Player_Brain_SoundsLayer Sounds => sounds;

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
                Managers.I.VillagerSelection.AddVillagerToSelect(other.gameObject.GetComponent<Villager>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Villager")) {
                Managers.I.VillagerSelection.RemoveVillagerToSelect(other.GetComponent<Villager>());
            }
        }
    }
}
