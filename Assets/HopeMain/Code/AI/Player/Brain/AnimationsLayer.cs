using System;
using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    public enum PlayerAnimationState
    {
        Idle, Run
    }
    
    public class AnimationsLayer : BrainLayer
    {
        [SerializeField] private Animator animator;
        
        private PlayerAnimationState currentState;

        private Action<SoundEffectType> playSoundEffectOnAnimation;
        
        public override void Initialize(Brain brain)
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
        
        private void PlaySoundEffectOnAnimation(SoundEffectType effectType) =>
            playSoundEffectOnAnimation.Invoke(effectType);

        
    }
}
