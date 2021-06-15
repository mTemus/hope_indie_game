using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float layerMultiplier;
        
        private Vector3 lastCameraPos;
        private float layerSizeX;
        
        public void Initialize(Transform cameraTransform)
        {
            lastCameraPos = cameraTransform.position;
            Sprite layerSprite = GetComponent<SpriteRenderer>().sprite;
            layerSizeX = layerSprite.texture.width / layerSprite.pixelsPerUnit;
        }

        public void Move(Vector3 currentCameraPosition)
        {
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            Vector3 deltaMovement = currentCameraPosition - lastCameraPos;
            
            myCurrentPosition += deltaMovement * layerMultiplier;
            myTransform.position = myCurrentPosition;
            lastCameraPos = currentCameraPosition;

            if (!(Mathf.Abs(currentCameraPosition.x - myCurrentPosition.x) >= layerSizeX)) return;
            float offsetPositionX = (currentCameraPosition.x - myCurrentPosition.x) % layerSizeX;
            myTransform.position = new Vector3(currentCameraPosition.x + offsetPositionX, myCurrentPosition.y);
        }
    }
}
