using System.Linq;
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
            if (debugAreas) 
                foreach (AreaDebug area in dAreas) 
                    area.ShowGrid(); 
        }

        private void ToggleGridText()
        {
            foreach (AreaDebug area in dAreas) 
                area.ToggleGridText(debugAreas); 
        }

        public void ToggleAreaDebugging()
        {
            debugAreas = !debugAreas;
            ToggleGridText();
        }

        public Area GetPlayerArea() => 
            areas.FirstOrDefault(area => area.IsPlayerInArea());
         

        public Area GetAreaByCoords(Vector3Int coordinates) =>
            (from area in areas let areaPosInt = Vector3Int.FloorToInt(area.transform.position) 
                let xAreaStart = areaPosInt.x 
                let xAreaEnd = areaPosInt.x + (int) area.Width 
                where coordinates.x > xAreaStart && coordinates.x < xAreaEnd 
                select area).FirstOrDefault();

        public Area GetVillageArea() =>
            areas.FirstOrDefault(area => area.Type == AreaType.VILLAGE);

        public void SetPlayerToArea(Area newArea, GameObject player)
        {
            GetPlayerArea().ClearPlayerInArea();;
            newArea.SetPlayerToArea(player);
            
            Debug.LogWarning("Player is in " + newArea.name);
        }
    }
}
