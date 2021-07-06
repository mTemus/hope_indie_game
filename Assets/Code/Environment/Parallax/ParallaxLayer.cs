using Code.System.Areas;
using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float layerMultiplier;
        
        private Vector3 lastCameraPos;
        private float layerSizeX;
        
        public void Initialize(Vector3 cameraPosition)
        {
            lastCameraPos = cameraPosition;
            Sprite layerSprite = GetComponent<SpriteRenderer>().sprite;
            layerSizeX = layerSprite.texture.width / layerSprite.pixelsPerUnit;
        }

        public void InitializeLocal(Vector3 cameraPos, Area area)
        {
            lastCameraPos = cameraPos;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            
            Vector2 newSpriteSize = new Vector2(area.Width * (1 - layerMultiplier), sr.size.y);
            sr.size = newSpriteSize;
            layerSizeX = newSpriteSize.x;
        }

        public void MoveLocally(Vector3 currentCameraPosition, Area area)
        {
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.localPosition;
            Vector3 deltaMovement = currentCameraPosition - lastCameraPos;
            
            myCurrentPosition += deltaMovement * layerMultiplier;
            
            float newXPos = area.ClampInArea(myCurrentPosition.x, layerSizeX);
            
            myTransform.position = new Vector3(newXPos, myCurrentPosition.y, myCurrentPosition.z);
            lastCameraPos = currentCameraPosition;
        }

        public void MoveGlobally(Vector3 currentCameraPosition)
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
