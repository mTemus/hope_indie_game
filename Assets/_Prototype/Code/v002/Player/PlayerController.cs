using UnityEngine;

namespace _Prototype.Code.v002.Player
{
    /// <summary>
    /// Class responsible for handling player character
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _motion;
        [SerializeField] private PlayerAnimations _animations;
        

        public PlayerMovement Motion => _motion;
        public PlayerAnimations Animations => _animations;
    }
}
