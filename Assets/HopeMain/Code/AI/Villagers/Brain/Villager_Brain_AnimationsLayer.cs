using System;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public enum VillagerAnimationState
    {
        Idle, Walk
    }
    
    public class Villager_Brain_AnimationsLayer : Villager_BrainLayer
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject spriteGo;

        private Action<VillagerSoundEffectType> playSoundEffectOnAnimation;

        private VillagerAnimationState currentState;
        
        private bool facingRight = true;

        private void Awake()
        {
            currentState = VillagerAnimationState.Idle;
        }

        public override void Initialize(Villager_Brain villagerBrain)
        {
            playSoundEffectOnAnimation = villagerBrain.Sounds.PlaySoundEffect;
        }

        public void SetState(VillagerAnimationState state)
        {
            if (currentState == state) return;
            currentState = state;
            
            animator.Play(currentState.ToString());
        }
        
        public void Turn(Vector3 position)
        {
            
            if (position.x >= transform.position.x) {
                if (facingRight) return;
                Flip();
            }
            else {
                if (!facingRight) return;
                Flip();
            }
        }
        
        private void Flip()
        {
            facingRight = !facingRight;

            Transform spriteGoTransform = spriteGo.transform;
            Vector3 theScale = spriteGoTransform.localScale;
            theScale.x *= -1;
            spriteGoTransform.localScale = theScale;
        }

        private void PlaySoundEffectOnAnimation(VillagerSoundEffectType effectType) =>
            playSoundEffectOnAnimation.Invoke(effectType);
    }
}
