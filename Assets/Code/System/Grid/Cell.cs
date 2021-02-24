using UnityEngine;

namespace Code.System.Grid
{
    public class Cell
    {
        private Transform buildingInCell;

        public void SetBuildingAtCell(Transform building) =>
            buildingInCell = building;
        
        public bool CanBuild() => 
            buildingInCell == null;
    }
}
