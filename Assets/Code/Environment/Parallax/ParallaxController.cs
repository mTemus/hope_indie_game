using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField] private List<ParallaxLayer> layers;
        
        private Transform cameraTransform;
        
        private void Awake()
        {
            if (Camera.main != null) 
                cameraTransform = Camera.main.transform;
            else 
                throw new Exception("PARALLAX CONTROLLER --- NO MAIN CAMERA TO SET");
        }
        
        void Start()
        {
            foreach (ParallaxLayer layer in layers) {
                layer.Initialize(cameraTransform);
            }
        }

        void LateUpdate()
        {
            Vector3 currentCameraPosition = cameraTransform.position;
            
            foreach (ParallaxLayer layer in layers) {
                layer.Move(currentCameraPosition);
            }
        }
    }
}
