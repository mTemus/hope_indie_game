using UnityEngine;

namespace Code.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
        private bool facingRight = true;
        
        public void Move(Vector3 direction)
        {
            transform.position += direction * (Time.deltaTime * speed);

            if (direction == Vector3.left) {
                if (!facingRight) return;
                Flip();
            }
            else {
                if (facingRight) return;
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
