using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.Environment.Parallax
{
    //TODO: clean this class
    
    /// <summary>
    /// 
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float layerMultiplier;
        [SerializeField] private float length;

        private Vector3 _lastCameraPos;
        private float _layerSizeX;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraPosition"></param>
        public void Initialize(Vector3 cameraPosition)
        {
            _lastCameraPos = cameraPosition;

            if (!TryGetComponent(out SpriteRenderer s)) return;
            Sprite layerSprite = s.sprite;
            _layerSizeX = layerSprite.texture.width / layerSprite.pixelsPerUnit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraPos"></param>
        /// <param name="area"></param>
        public void InitializeLocal(Vector3 cameraPos, Area area)
        {
            _lastCameraPos = cameraPos;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            
            Vector2 newSpriteSize = new Vector2(area.Width * (1 - layerMultiplier), sr.size.y);
            sr.size = newSpriteSize;
            _layerSizeX = newSpriteSize.x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentCameraPosition"></param>
        /// <param name="area"></param>
        public void MoveLocally(Vector3 currentCameraPosition, Area area)
        {
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.localPosition;
            Vector3 deltaMovement = currentCameraPosition - _lastCameraPos;
            
            myCurrentPosition += deltaMovement * layerMultiplier;
            
            float newXPos = area.ClampInArea(myCurrentPosition.x, _layerSizeX);
            
            myTransform.position = new Vector3(newXPos, myCurrentPosition.y, myCurrentPosition.z);
            _lastCameraPos = currentCameraPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentCameraPosition"></param>
        public void MoveGlobally(Vector3 currentCameraPosition)
        {
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            Vector3 deltaMovement = currentCameraPosition - _lastCameraPos;
            
            myCurrentPosition += deltaMovement * layerMultiplier;
            myTransform.position = myCurrentPosition;
            _lastCameraPos = currentCameraPosition;

            if (!(Mathf.Abs(currentCameraPosition.x - myCurrentPosition.x) >= _layerSizeX)) return;
            float offsetPositionX = (currentCameraPosition.x - myCurrentPosition.x) % _layerSizeX;
            myTransform.position = new Vector3(currentCameraPosition.x + offsetPositionX, myCurrentPosition.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currCamPos"></param>
        public void MoveOnWater(Vector3 currCamPos)
        {
            //TODO: try to implement this on other layer types
            
            Vector3 deltaMovement = currCamPos - _lastCameraPos;

            if (deltaMovement.x == 0) return;
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            
            myCurrentPosition += new Vector3(deltaMovement.x * layerMultiplier, 0, 0);
            myTransform.position = myCurrentPosition;

            if (Mathf.Abs(myCurrentPosition.x - currCamPos.x) >= length) 
                myTransform.position = deltaMovement.x > 0 ? 
                    new Vector3(myCurrentPosition.x + length * 2f, myCurrentPosition.y, -3) : 
                    new Vector3(myCurrentPosition.x - length * 2f, myCurrentPosition.y, -3); 
            
            _lastCameraPos = currCamPos;
        }
    }
}
