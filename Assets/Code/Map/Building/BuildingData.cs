using System.Collections.Generic;
using Code.Resources;
using UnityEngine;

namespace Code.Map.Building
{
    [CreateAssetMenu(fileName = "Building Data", menuName = "Game Data/Map Objects/Building Data", order = 0)]
    public class BuildingData : ScriptableObject
    {
        [Header("Properties")] 
        [SerializeField] private BuildingType buildingType;
        [SerializeField] private Transform prefab;
        [SerializeField] private string buildingName;
        
        [Header("Size")]
        [SerializeField] private Vector2Int size;
        [SerializeField] private Vector3 entrancePivot;

        [Header("Resources")]
        [SerializeField] private Resource[] requiredResources;

        public List<Vector2Int> GetOffset(Vector2Int pivot)
        {
            List<Vector2Int> offsetPositions = new List<Vector2Int>();

            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    offsetPositions.Add(pivot + new Vector2Int(x, y));
                }
            }

            return offsetPositions;
        }

        public BuildingType BuildingType => buildingType;

        public Transform Prefab => prefab;

        public string BuildingName => buildingName;
        
        public Vector2Int Size => size;

        public Vector3 EntrancePivot => entrancePivot;

        public Resource[] RequiredResources => requiredResources;
    }
}