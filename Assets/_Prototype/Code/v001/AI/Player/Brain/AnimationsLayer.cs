using System;
using _Prototype.Code.v002.Player;
using UnityEngine;

namespace _Prototype.Code.v001.AI.Player.Brain
{

    
    /// <summary>
    /// 
    /// </summary>
    public class AnimationsLayer : BrainLayer
    {
        [SerializeField] private Animator animator;
        
        private PlayerAnimationState _currentState;

        private Action<SoundEffectType> _playSoundEffectOnAnimation;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brain"></param>
        public override void Initialize(Brain brain)
        {
            _playSoundEffectOnAnimation = brain.Sounds.PlaySoundEffect;
            
        }
        
        private void Awake()
        {
            _currentState = PlayerAnimationState.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SetState(PlayerAnimationState state)
        {
            if (_currentState == state) return;
            _currentState = state;
            
            animator.Play(_currentState.ToString());
        }
        
        private void PlaySoundEffectOnAnimation(SoundEffectType effectType) =>
            _playSoundEffectOnAnimation.Invoke(effectType);

        
    }
}
