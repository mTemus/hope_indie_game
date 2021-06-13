using System;
using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float layerMultiplier;

        private Transform cameraTransform;

        private Vector3 lastCameraPos;
        private float layerSizeX;

        private void Awake()
        {
            if (Camera.main != null) 
                cameraTransform = Camera.main.transform;
            else 
                throw new Exception("PARALLAX LAYER --- NO MAIN CAMERA TO SET");
        }

        private void Start()
        {
            lastCameraPos = cameraTransform.position;
            Sprite layerSprite = GetComponent<SpriteRenderer>().sprite;
            layerSizeX = layerSprite.texture.width / layerSprite.pixelsPerUnit;
        }

        private void Update()
        {
            Transform myTransform = transform;
            Vector3 myCurrentPosition = myTransform.position;
            Vector3 currentCameraPosition = cameraTransform.position;
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
