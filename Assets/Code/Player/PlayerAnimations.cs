using System;
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
        
        private PlayerAnimationState currentState;

        private void Awake()
        {
            currentState = PlayerAnimationState.Idle;
        }

        public void SetState(PlayerAnimationState state)
        {
            if (currentState == state) return;
            currentState = state;
            
            
            Debug.Log(currentState.ToString());
            animator.Play(currentState.ToString());
        }
    }
}
