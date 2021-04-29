using Code.Map.Building;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeBuilding : InitializeObject
    {
        public override void InitializeMe()
        {
            Building building = GetComponent<Building>();
            Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .AddBuilding(building, building.Data);
            Managers.Instance.Buildings.AddBuilding(building.Data.BuildingType, building);
        }
    }
}
