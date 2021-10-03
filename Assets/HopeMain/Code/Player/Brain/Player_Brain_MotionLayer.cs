using UnityEngine;

namespace Code.Player.Brain
{
    public class Player_Brain_MotionLayer : Player_Brain_Layer
    {
        public float speed = 10f;
        private bool facingRight = true;
        
        public override void Initialize(Player_Brain brain) { }

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
