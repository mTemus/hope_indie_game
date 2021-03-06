using _Prototype.Code.v001.World.Grid.Cell;
using _Prototype.Code.v001.World.Resources;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "Building Data", menuName = "Game Data/Map Objects/Building Data", order = 0)]
    public class Data : ScriptableObject
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