using UnityEngine;

namespace _Prototype.Code.v002.Player
{
    /// <summary>
    /// State machine of player character animations
    /// </summary>
    public enum PlayerAnimationState
    {
        Idle,
        Run
    }

    /// <summary>
    /// Class responsible for handling animations of player character
    /// </summary>
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private  Animator _animator;
        
        private PlayerAnimationState _currentState;
        
        /// <summary>
        /// Set player animation by giving an PlayerAnimationState enum value
        /// </summary>
        /// <param name="state">PlayerAnimationState enum value</param>
        public void SetState(PlayerAnimationState state)
        {
            if (_currentState == state) return;
            _currentState = state;
            _animator.Play(_currentState.ToString());
        }
    }
}