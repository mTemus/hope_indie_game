using System;
using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    /// <summary>
    /// State machine of player character animations
    /// </summary>
    public enum PlayerAnimationState
    {
        Idle, Run
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class AnimationsLayer : BrainLayer
    {
        [SerializeField] private Animator animator;

        private static float valueOne;
        private float valueTwo;
        
        [Range(valueOne,3)]
        public int someValue;
        
        
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
