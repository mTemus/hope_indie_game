using UnityEngine;

namespace Code.Villagers.Brain.Layers
{
    public enum VillagerAnimationState
    {
        Idle, Walk
    }
    
    public class Villager_Brain_AnimationsLayer : BrainLayer
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject spriteGo;

        private VillagerAnimationState currentState;
        private Villager_Brain_SoundsLayer sounds;
        
        private bool facingRight = true;

        private void Awake()
        {
            currentState = VillagerAnimationState.Idle;
        }

        public override void Initialize(Villager_Brain villagerBrain)
        {
            base.Initialize(villagerBrain);
            sounds = villagerBrain.Sounds;
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
            sounds.PlaySoundEffect(effectType);
    }
}