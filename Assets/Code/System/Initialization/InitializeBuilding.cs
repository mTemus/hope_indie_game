using Code.Map.Building;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeBuilding : InitializeObject
    {
        [SerializeField] private BuildingScript buildingData;
        
        public override void InitializeMe()
        {
            Building building = GetComponent<Building>();
            building.SetEntrancePivot(buildingData.xPivot, buildingData.yPivot, 0f);
            Area.Area myArea = Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            myArea.AddBuilding(building, buildingData);
        }
    }
}
