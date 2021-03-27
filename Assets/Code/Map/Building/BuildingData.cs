using System.Collections.Generic;
using Code.Resources;
using UnityEngine;

namespace Code.Map.Building
{
    [CreateAssetMenu(fileName = "Building Data", menuName = "Game Data/Map Objects/Building Data", order = 0)]
    public class BuildingData : ScriptableObject
    {
        [Header("Properties")]
        [SerializeField] private Transform prefab;
        [SerializeField] private string buildingName;
        [SerializeField] private int maxOccupancy;
        
        [Header("Size")]
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float xPivot;
        [SerializeField] private float yPivot;
        
        [Header("Resources")]
        [SerializeField] private Resource[] requiredResources;

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

        public Transform Prefab => prefab;

        public string BuildingName => buildingName;

        public int MAXOccupancy => maxOccupancy;

        public int Width => width;

        public int Height => height;

        public float XPivot => xPivot;

        public float YPivot => yPivot;

        public Resource[] RequiredResources => requiredResources;
    }
}