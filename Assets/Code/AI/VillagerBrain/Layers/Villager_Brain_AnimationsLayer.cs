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
            
            
            Debug.Log(currentState.ToString());
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

            var myTransform = transform;
            Vector3 theScale = myTransform.localScale;
            theScale.x *= -1;
            myTransform.localScale = theScale;
        }
    }
}
