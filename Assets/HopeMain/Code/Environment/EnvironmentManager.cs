using HopeMain.Code.Environment.Parallax;
using UnityEngine;

namespace HopeMain.Code.Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        [Header("Camera")] 
        [SerializeField] private Camera mainCamera;
        
        [Header("Parallax")] 
        [SerializeField] private GlobalParallaxController globalParallax;
        [SerializeField] private LocalParallaxController localParallax;
        [SerializeField] private WaterParallaxController waterParallax;
        
        public GlobalParallaxController GlobalParallax => globalParallax;
        public LocalParallaxController LocalParallax => localParallax;
        public WaterParallaxController WaterParallax => waterParallax;
        
        private void Awake()
        {
            globalParallax.InitializeLayers(mainCamera.transform.position);
        }

        public void SetLocalParallax(LocalParallaxController parallaxController)
        {
            localParallax = parallaxController;
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
