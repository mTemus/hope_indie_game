using System.Collections.Generic;
using Code.Resources;
using UnityEngine;

namespace Code.Map.Building
{
    [CreateAssetMenu(fileName = "Building Data", menuName = "Map Objects/Building Data", order = 0)]
    public class BuildingData : ScriptableObject
    {
        public Transform prefab;
        
        public string buildingName;
        public int width;
        public int height;
        public float xPivot;
        public float yPivot;
        public Resource[] requiredResources;

        public List<Vector2Int> GetOffset(Vector2Int pivot)
        {
            List<Vector2Int> offsetPositions = new List<Vector2Int>();

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    offsetPositions.Add(pivot + new Vector2Int(x, y));
                }
            }

            return offsetPositions;
        }
        
    }
}