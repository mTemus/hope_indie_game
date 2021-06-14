using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_MotionLayer : BrainLayer
    {
        [SerializeField] private float speed = 5f;
        
        private bool facingRight = true;
        private bool turnedFacing = false;
        
        public bool MoveTo(Vector3 position)
        {
            if (!turnedFacing) 
                Turn(position);

            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, speed * Time.deltaTime);
            
            transform.position = villagerPosition;
            bool isOnPosition = villagerPosition == position;

            if (isOnPosition) 
                turnedFacing = false;
            
            return isOnPosition;
        }
        
        public bool MoveTo(Vector3 position, float villagerSpeed)
        {
            if (!turnedFacing) 
                Turn(position);
            
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, villagerSpeed * Time.deltaTime);
            
            transform.position = villagerPosition;
            bool isOnPosition = villagerPosition == position;

            if (isOnPosition) 
                turnedFacing = false;
            
            return isOnPosition;
        }

        private void Turn(Vector3 position)
        {
            turnedFacing = true;
            
            if (position.x >= transform.position.x) {
                if (facingRight) return;
                Flip();
            }
            else {
                if (!facingRight) return;
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
