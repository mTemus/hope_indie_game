using Code.Map.Building;
using Code.Map.Resources.ResourceToGather;

namespace Code.System.Grid
{
    public class Cell
    {
        public Building buildingData;
        public ResourceToGather resourceToGatherData;

        public bool CanBuild() =>
            buildingData == null;
    }
}
