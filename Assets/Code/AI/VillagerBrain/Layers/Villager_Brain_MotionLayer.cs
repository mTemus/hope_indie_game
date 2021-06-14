using System;
using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public class Villager_Brain_MotionLayer : BrainLayer
    {
        [SerializeField] private float speed = 5f;

        private Action<Vector3> onVillagerTurnDirection;
        
        private bool turnedFacing = false;

        public override void Initialize(Villager_Brain villagerBrain)
        {
            base.Initialize(villagerBrain);
            onVillagerTurnDirection += villagerBrain.Animations.Turn;
        }

        public bool MoveTo(Vector3 position)
        {
            if (!turnedFacing) {
                onVillagerTurnDirection.Invoke(position);
                turnedFacing = true;
            }

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
            if (!turnedFacing) {
                onVillagerTurnDirection.Invoke(position);
                turnedFacing = true;
            }
            
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, villagerSpeed * Time.deltaTime);
            
            transform.position = villagerPosition;
            bool isOnPosition = villagerPosition == position;

            if (isOnPosition) 
                turnedFacing = false;
            
            return isOnPosition;
        }
    }
}
