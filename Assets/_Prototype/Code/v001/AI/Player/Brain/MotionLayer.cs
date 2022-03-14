using UnityEngine;

namespace _Prototype.Code.v001.AI.Player.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public class MotionLayer : BrainLayer
    {
        public float speed = 10f;
        private bool _facingRight = true;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brain"></param>
        public override void Initialize(Brain brain) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
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
        
        private void Flip()
        {
            _facingRight = !_facingRight;

            var myTransform = transform;
            Vector3 theScale = myTransform.localScale;
            theScale.x *= -1;
            myTransform.localScale = theScale;
        }
    }
}
