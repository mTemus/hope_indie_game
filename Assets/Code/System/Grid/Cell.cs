using Code.Map.Building;
using Code.Map.Resources.ResourceToGather;

namespace Code.System.Grid
{
    public enum CellContentType
    {
        Null, Nothing, WoodResource, StoneResource, Building, 
    }
    
    public abstract class Cell
    {
        public Building buildingData;
        public ResourceToGather resourceToGatherData;
        public CellContentType content;

        protected Cell()
        {
            content = CellContentType.Nothing;
        }

        public bool CanBePlacedOn(CellContentType requiredType) =>
            content == requiredType;

        public bool ContainsBuilding() =>
            buildingData != null;

        public bool ContainsResource() =>
            resourceToGatherData != null;
    }
}
