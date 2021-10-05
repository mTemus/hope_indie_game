using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    public class MotionLayer : BrainLayer
    {
        public float speed = 10f;
        private bool facingRight = true;
        
        public override void Initialize(Brain brain) { }

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
