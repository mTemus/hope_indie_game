using Code.System.Areas;
using UnityEngine;

namespace Code.Environment.Parallax
{
    //TODO: clean this class
    
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float layerMultiplier;
        [SerializeField] private float length;

        private Vector3 lastCameraPos;
        private float layerSizeX;
        
        public void Initialize(Vector3 cameraPosition)
        {
            lastCameraPos = cameraPosition;

            if (!TryGetComponent(out SpriteRenderer s)) return;
            Sprite layerSprite = s.sprite;
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

        public void MoveOnWater(Vector3 currCamPos)
        {
            //TODO: try to implement this on other layer types
            
            Vector3 deltaMovement = currCamPos - lastCameraPos;

            if (deltaMovement.x == 0) return;
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            
            myCurrentPosition += new Vector3(deltaMovement.x * layerMultiplier, 0, 0);
            myTransform.position = myCurrentPosition;

            if (Mathf.Abs(myCurrentPosition.x - currCamPos.x) >= length) 
                myTransform.position = deltaMovement.x > 0 ? 
                    new Vector3(myCurrentPosition.x + length * 2f, myCurrentPosition.y, -3) : 
                    new Vector3(myCurrentPosition.x - length * 2f, myCurrentPosition.y, -3); 
            
            lastCameraPos = currCamPos;
        }
    }
}
