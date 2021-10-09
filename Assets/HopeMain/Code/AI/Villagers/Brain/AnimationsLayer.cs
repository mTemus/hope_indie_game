using System;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public enum VillagerAnimationState
    {
        Idle, Walk
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class AnimationsLayer : BrainLayer
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject spriteGo;

        private Action<SoundEffectType> _onPlaySoundEffectOnAnimation;

        private VillagerAnimationState _currentState;
        
        private bool _facingRight = true;

        private void Awake()
        {
            _currentState = VillagerAnimationState.Idle;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brain"></param>
        public override void Initialize(Brain brain)
        {
            _onPlaySoundEffectOnAnimation = brain.Sounds.PlaySoundEffect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SetState(VillagerAnimationState state)
        {
            if (_currentState == state) return;
            _currentState = state;
            
            animator.Play(_currentState.ToString());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void Turn(Vector3 position)
        {
            
            if (position.x >= transform.position.x) {
                if (_facingRight) return;
                Flip();
            }
            else {
                if (!_facingRight) return;
                Flip();
            }
        }
        
        private void Flip()
        {
            _facingRight = !_facingRight;

            Transform spriteGoTransform = spriteGo.transform;
            Vector3 theScale = spriteGoTransform.localScale;
            theScale.x *= -1;
            spriteGoTransform.localScale = theScale;
        }

        private void PlaySoundEffectOnAnimation(SoundEffectType effectType) =>
            _onPlaySoundEffectOnAnimation.Invoke(effectType);
    }
}
