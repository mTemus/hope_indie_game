using Code.Map.Building;

namespace Code.System.Grid
{
    public class Cell
    {
        private Building buildingData;

        public void SetBuildingAtCell(Building building) =>
            buildingData = building;

        public bool CanBuild() =>
            buildingData == null;
    }
}
