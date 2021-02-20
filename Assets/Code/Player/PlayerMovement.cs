using UnityEngine;

namespace Code.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
    
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
                transform.position += Vector3.left * (Time.deltaTime * speed);
        
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
                transform.position += Vector3.right * (Time.deltaTime * speed);
        }
    }
}
