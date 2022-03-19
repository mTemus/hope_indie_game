using UnityEngine;

namespace _Prototype.Code.v002.Player
{
    /// <summary>
    /// Class responsible for handling player character
    /// </summary>
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerAnimations animations;
        
        public PlayerMovement Movement => movement;
        public PlayerAnimations Animations => animations;
        
    }
}