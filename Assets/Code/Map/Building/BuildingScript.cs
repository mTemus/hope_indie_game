using System.Collections.Generic;
using UnityEngine;

namespace Code.Map.Building
{
    [CreateAssetMenu(fileName = "Building Data", menuName = "Map Objects", order = 0)]
    public class BuildingScript : ScriptableObject
    {
        public Transform prefab;
        
        public string buildingName;
        public int width;
        public int height;
        public float xPivot;
        public float yPivot;

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