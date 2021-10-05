using System;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public class MotionLayer : BrainLayer
    {
        [SerializeField] private float speed = 5f;

        private Action<Vector3> onVillagerTurnDirection;
        
        private bool turnedFacing;

        public override void Initialize(Brain brain)
        {
            onVillagerTurnDirection += brain.Animations.Turn;
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
