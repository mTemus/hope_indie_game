using _Prototype.Code.v002.Player.Tools;
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
        [SerializeField] private PlayerTools tools;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            animations = GetComponent<PlayerAnimations>();
        }

        public PlayerMovement Movement => movement;
        public PlayerAnimations Animations => animations;
        public PlayerTools Tools => tools;
    }
}