using UnityEngine;

namespace Code.Player
{
    public enum PlayerAnimationState
    {
        Idle, Run
    }
    
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private PlayerSoundEffects sounds;

        private PlayerAnimationState currentState;

        private void Awake()
        {
            sounds = transform.parent.gameObject.GetComponent<PlayerController>().Sounds;
            currentState = PlayerAnimationState.Idle;
        }

        public void SetState(PlayerAnimationState state)
        {
            if (currentState == state) return;
            currentState = state;
            
            animator.Play(currentState.ToString());
        }
        
        private void PlaySoundEffectOnAnimation(PlayerSoundEffectType effectType) =>
            sounds.PlaySoundEffect(effectType);
    }
}
