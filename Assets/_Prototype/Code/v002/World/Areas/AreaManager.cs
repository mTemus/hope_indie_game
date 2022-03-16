using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototype.Code.v002.World.Areas
{
    /// <summary>
    /// Class managing area objects on current scene
    /// </summary>
    public class AreaManager : MonoBehaviour
    {
        [SerializeField] private bool debugAreas;
        [SerializeField] private Area[] areas;
        
        private AreaDebug[] _debugAreas;
        
        private void Start()
        {
            _debugAreas = new AreaDebug[areas.Length];
            
            for (int i = 0; i < areas.Length; i++) 
            {
                AreaDebug dArea = areas[i].GetComponent<AreaDebug>();
                dArea.CreateGridText();
                _debugAreas[i] = dArea;
            }

            if (debugAreas) return;
            ToggleGridText();
        }

        private void Update()
        {
            if (!debugAreas) return;
            foreach (AreaDebug area in _debugAreas) 
                area.ShowGrid();
        }
        
        private void ToggleGridText()
        {
            foreach (AreaDebug area in _debugAreas) 
                area.ToggleGridText(debugAreas); 
        }

        /// <summary>
        /// Shows/Hides grid debug gizmos and text
        /// </summary>
        public void ToggleAreaDebugging()
        {
            debugAreas = !debugAreas;
            ToggleGridText();
        }

        /// <summary>
        /// Get Area object based that has given coordinates in it
        /// </summary>
        /// <param name="coordinates">World coordinates</param>
        /// <returns>Area object</returns>
        public Area GetAreaByCoords(Vector3Int coordinates)
        {
            foreach (Area area in areas) {
                Vector3Int areaPos = Vector3Int.FloorToInt(area.transform.position);
                int xAreaEnd = areaPos.x + area.Width;

                if (coordinates.x >= areaPos.x && coordinates.x <= xAreaEnd)
                    return area;
            }
            return null;
        }

        /// <summary>
        /// Get Area object with areaType 'Village'
        /// </summary>
        /// <returns>Area object</returns>
        public Area GetVillageArea()
        {
            foreach (Area area in areas)
                return area.Type == AreaType.Village ? area : null;

            return null;
        }

        /// <summary>
        /// Get area objects of given area type
        /// </summary>
        /// <param name="areaType">Type of areas that should be returned</param>
        /// <returns>Array of 'Area' objects</returns>
        public Area[] FindAllAreaByType(AreaType areaType)
        {
            List<Area> allAreas = new List<Area>();
            foreach (Area area in areas) 
                if(area.Type == areaType) allAreas.Add(area);

            return allAreas.ToArray();
        }
        
        /// <summary>
        /// Get Area object of given type closest to given point in the game world
        /// </summary>
        /// <param name="position">Game world point position</param>
        /// <param name="areaType">Wanted area type</param>
        /// <returns>Area object</returns>
        /// <exception cref="NoAreasOfTypeException">If there is no area with given type</exception>
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
    }
}
