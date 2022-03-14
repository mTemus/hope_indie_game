using System;
using UnityEngine;

namespace _Prototype.Code.v001.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public class MotionLayer : BrainLayer
    {
        [SerializeField] private float speed = 5f;

        private Action<Vector3> _villagerTurnDirection;
        
        private bool _turnedFacing;
        
        public override void Initialize(Brain brain)
        {
            _villagerTurnDirection += brain.Animations.Turn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool MoveTo(Vector3 position)
        {
            if (!_turnedFacing) {
                _villagerTurnDirection.Invoke(position);
                _turnedFacing = true;
            }

            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, speed * Time.deltaTime);
            
            transform.position = villagerPosition;
            bool isOnPosition = villagerPosition == position;

            if (isOnPosition) 
                _turnedFacing = false;
            
            return isOnPosition;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="villagerSpeed"></param>
        /// <returns></returns>
        public bool MoveTo(Vector3 position, float villagerSpeed)
        {
            if (!_turnedFacing) {
                _villagerTurnDirection.Invoke(position);
                _turnedFacing = true;
            }
            
            Vector3 villagerPosition = transform.position;
            villagerPosition = Vector3.MoveTowards(villagerPosition , position, villagerSpeed * Time.deltaTime);
            
            transform.position = villagerPosition;
            bool isOnPosition = villagerPosition == position;

            if (isOnPosition) 
                _turnedFacing = false;
            
            return isOnPosition;
        }
    }
}
