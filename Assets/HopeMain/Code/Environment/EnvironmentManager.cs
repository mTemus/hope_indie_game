using HopeMain.Code.Environment.Parallax;
using UnityEngine;

namespace HopeMain.Code.Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        [Header("Camera")] 
        [SerializeField] private Camera mainCamera;
        
        [Header("Parallax")] 
        [SerializeField] private ParallaxControllerGlobal globalParallax;
        [SerializeField] private ParallaxControllerLocal localParallax;
        [SerializeField] private ParallaxControllerWater waterParallax;
        
        public ParallaxControllerGlobal GlobalParallax => globalParallax;
        public ParallaxControllerLocal LocalParallax => localParallax;
        public ParallaxControllerWater WaterParallax => waterParallax;
        
        private void Awake()
        {
            globalParallax.InitializeLayers(mainCamera.transform.position);
        }

        public void SetLocalParallax(ParallaxControllerLocal parallax)
        {
            localParallax = parallax;
            localParallax.InitializeLayers(mainCamera.transform.position);
        }
        
        private void LateUpdate()
        {
            Vector3 currCamPos = mainCamera.transform.position;
            
            globalParallax.Move(currCamPos);
            waterParallax.Move(currCamPos);
            
            if (localParallax == null) return;
            localParallax.Move(currCamPos);
        }
    }
}
