using UnityEngine;

namespace _Prototype.Code.v002.Player
{
    /// <summary>
    /// Class responsible for handling movement of player character
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;

        private bool _facingRight = true;

        private void Flip()
        {
            _facingRight = !_facingRight;

            Transform myTransform = transform;
            Vector3 theScale = myTransform.localScale;
            theScale.x *= -1;
            myTransform.localScale = theScale;
        }
    
        /// <summary>
        /// Move player character
        /// </summary>
        /// <param name="direction">direction that player object should go in</param>
        public void Move(Vector3 direction)
        {
            transform.position += direction * (Time.deltaTime * speed);

            if (direction == Vector3.left) {
                if (!_facingRight) return;
                Flip();
            }
            else {
                if (_facingRight) return;
                Flip();
            }
        }
    }
}
