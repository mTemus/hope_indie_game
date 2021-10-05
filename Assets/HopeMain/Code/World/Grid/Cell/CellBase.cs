using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Resources.ResourceToGather;

namespace HopeMain.Code.World.Grid.Cell
{
    public enum CellContentType
    {
        Null, Nothing, WoodResource, StoneResource, Building, 
    }
    
    public abstract class CellBase
    {
        public Building buildingData;
        public ResourceToGatherBase resourceToGatherData;
        public CellContentType content;

        protected CellBase()
        {
            content = CellContentType.Nothing;
        }
        
        public bool ContainsBuilding() =>
            buildingData != null;

        public bool ContainsResource() =>
            resourceToGatherData != null;
    }
}
