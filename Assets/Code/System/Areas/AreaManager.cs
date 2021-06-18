using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.System.Areas
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
            if (!debugAreas) return;
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
            areas.FirstOrDefault(area => area.IsPlayerInArea);
        
        public Area GetAreaByCoords(Vector3Int coordinates) =>
            (from area in areas let areaPosInt = Vector3Int.FloorToInt(area.transform.position) 
                let xAreaStart = areaPosInt.x 
                let xAreaEnd = areaPosInt.x + (int) area.Width 
                where coordinates.x > xAreaStart && coordinates.x < xAreaEnd 
                select area).FirstOrDefault();

        public Area GetVillageArea() =>
            areas.FirstOrDefault(area => area.Type == AreaType.VILLAGE);

        public Area[] FindAllAreaByType(AreaType areaType) =>
            areas.Where(area => area.Type == areaType)
                .ToArray();

        public Area FindClosestAreaOfType(Vector3 position, AreaType areaType)
        {
            Area[] areasOfType = FindAllAreaByType(areaType);

            if (areas.Length == 0) 
                throw new Exception("NO AREAS OF TYPE: " + areaType);

            Area closestArea = areasOfType[0];
            float bestDistance = Vector3.Distance(position, closestArea.transform.position);
            
            foreach (Area area in areasOfType) {
                float distance = Vector3.Distance(position, area.transform.position);

                if (bestDistance < distance) continue;
                bestDistance = distance;
                closestArea = area;
            }

            return closestArea;
        }

        public Area FindClosestAreaOfTypes(Vector3 position, AreaType[] areaTypes)
        {
            List<Area> areasToFilter = new List<Area>();

            foreach (AreaType areaType in areaTypes) 
                areasToFilter.AddRange(FindAllAreaByType(areaType));
            
            Area closestArea = areasToFilter[0];
            float bestDistance = Vector3.Distance(position, closestArea.transform.position);
            
            foreach (Area area in areasToFilter) {
                float distance = Vector3.Distance(position, area.transform.position);

                if (bestDistance < distance) continue;
                bestDistance = distance;
                closestArea = area;
            }

            return closestArea;
        }
    }
}
