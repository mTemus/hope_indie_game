using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Resources.ResourceToGather;

namespace HopeMain.Code.World.Grid
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
        
        public bool ContainsBuilding() =>
            buildingData != null;

        public bool ContainsResource() =>
            resourceToGatherData != null;
    }
}
