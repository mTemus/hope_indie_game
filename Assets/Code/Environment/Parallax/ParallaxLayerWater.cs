using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxLayerWater : MonoBehaviour
    {
        public float length;
        public float parallaxMultiplier;
        
        private Camera camera;
        private Vector3 lastCamPos;

        private void Start()
        {
            camera = Camera.main;
            lastCamPos = camera.transform.position;
        }

        private void LateUpdate()
        {
            Vector3 currCamPos = camera.transform.position;
            Vector3 deltaMovement = currCamPos - lastCamPos;

            if (deltaMovement.x == 0) return;
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            
            myCurrentPosition += new Vector3(deltaMovement.x * parallaxMultiplier, 0, 0);
            myTransform.position = myCurrentPosition;

            if (Mathf.Abs(myCurrentPosition.x - currCamPos.x) >= length) 
                myTransform.position = deltaMovement.x > 0 ? 
                new Vector3(myCurrentPosition.x + length * 2f, myCurrentPosition.y, -3) : 
                new Vector3(myCurrentPosition.x - length * 2f, myCurrentPosition.y, -3); 
            
            lastCamPos = currCamPos;
        }
    }
}
