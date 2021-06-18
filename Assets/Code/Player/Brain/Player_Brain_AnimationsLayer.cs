using System;
using UnityEngine;

namespace Code.Player.Brain
{
    public enum PlayerAnimationState
    {
        Idle, Run
    }
    
    public class Player_Brain_AnimationsLayer : Player_Brain_Layer
    {
        [SerializeField] private Animator animator;
        
        private PlayerAnimationState currentState;

        private Action<PlayerSoundEffectType> playSoundEffectOnAnimation;
        
        public override void Initialize(Player_Brain brain)
        {
            playSoundEffectOnAnimation = brain.Sounds.PlaySoundEffect;
        }
        
        private void Awake()
        {
            currentState = PlayerAnimationState.Idle;
        }

        public void SetState(PlayerAnimationState state)
        {
            if (currentState == state) return;
            currentState = state;
            
            animator.Play(currentState.ToString());
        }
        
        private void PlaySoundEffectOnAnimation(PlayerSoundEffectType effectType) =>
            playSoundEffectOnAnimation.Invoke(effectType);

        
    }
}
