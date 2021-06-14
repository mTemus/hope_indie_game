using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
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

        private bool facingRight = true;

        private void Awake()
        {
            currentState = VillagerAnimationState.Idle;
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
    }
}
