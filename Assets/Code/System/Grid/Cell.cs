using Code.Map.Building;
using UnityEngine;

namespace Code.System.Grid
{
    public class Cell
    {
        private BuildingData buildingData;

        public Cell()
        {
            buildingData = new BuildingData();
        }

        public void SetBuildingAtCell(Transform building) =>
            buildingData.BuildingObject = building;

        public void SetBuildingScriptAtCell(BuildingScript buildingScript) =>
            buildingData.BuildingScript = buildingScript;
        
        public bool CanBuild() => 
            buildingData.BuildingObject == null && buildingData.BuildingScript == null;
    }
}
