using Code.Map.Building;
using UnityEngine;

namespace Code.System.Grid
{
    public class Cell
    {
        private readonly BuildingData buildingData;

        public Cell()
        {
            buildingData = new BuildingData();
        }

        public void SetBuildingAtCell(Transform building) =>
            buildingData.BuildingObject = building;

        public bool CanBuild() =>
            buildingData.BuildingObject == null;
    }
}
