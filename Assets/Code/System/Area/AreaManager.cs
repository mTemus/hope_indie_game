using UnityEngine;

namespace Code.System.Area
{
    public class AreaManager : MonoBehaviour
    {
        [SerializeField] private bool debugAreas;
        [SerializeField] private Area[] areas;

        private AreaDebug[] dAreas;
    
        private void Start()
        {
            dAreas = new AreaDebug[areas.Length];
            
            for (int i = 0; i < areas.Length; i++) 
            {
                AreaDebug dArea = areas[i].GetComponent<AreaDebug>();
                dArea.CreateGridText();
                dAreas[i] = dArea;
            }

            if (debugAreas) return;
            ToggleGridText();
        }

        private void Update()
        {
            if (debugAreas) {
                foreach (AreaDebug area in dAreas) { area.ShowGrid(); }
            }
        }

        private void ToggleGridText()
        {
            foreach (AreaDebug area in dAreas) { area.ToggleGridText(debugAreas); }
        }

        public void ToggleAreaDebugging()
        {
            debugAreas = !debugAreas;
            ToggleGridText();
        }
    }
}
