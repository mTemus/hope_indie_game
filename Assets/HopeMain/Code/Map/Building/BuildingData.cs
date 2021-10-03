using Code.Map.Resources;
using Code.System.Grid;
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
        [SerializeField] private CellContentType requiredCellContent;
        
        [Header("Size")]
        [SerializeField] private Vector2Int size;
        [SerializeField] private Vector3 entrancePivot;

        [Header("Resources")]
        [SerializeField] private Resource[] requiredResources;

        public CellContentType RequiredCellContent => requiredCellContent;
        public BuildingType BuildingType => buildingType;
        public Transform Prefab => prefab;
        public string BuildingName => buildingName;
        public Vector2Int Size => size;
        public Vector3 EntrancePivot => entrancePivot;
        public Resource[] RequiredResources => requiredResources;
    }
}