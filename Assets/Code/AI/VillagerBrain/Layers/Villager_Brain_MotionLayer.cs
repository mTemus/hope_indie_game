using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_MotionLayer : BrainLayer
    {
        [SerializeField] private float speed = 5f;
        
        public bool MoveTo(Vector3 position)
        {
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, speed * Time.deltaTime);
            
            transform.position = villagerPosition;

            return villagerPosition == position;
        }
        
        public bool MoveTo(Vector3 position, float villagerSpeed)
        {
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, villagerSpeed * Time.deltaTime);
            
            transform.position = villagerPosition;

            return villagerPosition == position;
        }
        
    }
}
