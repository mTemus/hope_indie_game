using Code.Map.Building;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeBuilding : InitializeObject
    {
        [SerializeField] private BuildingData buildingData;
        
        public override void InitializeMe()
        {
            Building building = GetComponent<Building>();
            building.Data = buildingData;
            Area.Area myArea = Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            myArea.AddBuilding(building, buildingData);
            Managers.Instance.Buildings.AddBuilding(buildingData.BuildingType, building);
        }
    }
}
