using UnityEngine;

namespace Code.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;

        public void Move(Vector3 direction) =>
            transform.position += direction * (Time.deltaTime * speed);
        
    }
}
