using System;
using Code.Environment.Parallax;
using UnityEngine;

namespace Code.Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        [Header("Camera")] 
        [SerializeField] private Camera mainCamera;
        
        [Header("Parallax")] 
        [SerializeField] private ParallaxControllerGlobal globalParallax;
        [SerializeField] private ParallaxControllerLocal localParallax;
        
        public ParallaxControllerGlobal GlobalParallax => globalParallax;
        public ParallaxControllerLocal LocalParallax => localParallax;

        private void Awake()
        {
            globalParallax.InitializeLayers(mainCamera.transform.position);
        }

        public void SetLocalParallax(ParallaxControllerLocal parallax)
        {
            localParallax = parallax;
            localParallax.InitializeLayers(localParallax.transform.InverseTransformVector(mainCamera.transform.position));
        }
        
        private void LateUpdate()
        {
            Vector3 currCamPos = mainCamera.transform.position;
            
            globalParallax.Move(currCamPos);
            
            if (localParallax == null) return;
            localParallax.Move(localParallax.transform.InverseTransformVector(currCamPos));
        }
    }
}
