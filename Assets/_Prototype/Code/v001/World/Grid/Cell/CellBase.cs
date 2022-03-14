using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Resources.ResourceToGather;

namespace _Prototype.Code.v001.World.Grid.Cell
{
    /// <summary>
    /// 
    /// </summary>
    public enum CellContentType
    {
        Null, Nothing, WoodResource, StoneResource, Building, 
    }
    
    /// <summary>
    /// 
    /// </summary>
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
