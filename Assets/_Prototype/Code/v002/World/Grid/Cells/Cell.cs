using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Resources.ResourceToGather;

namespace _Prototype.Code.v002.World.Grid.Cells
{
    /// <summary>
    /// Describing what type of object cell is containing
    /// </summary>
    public enum CellContentType
    {
        Null, Nothing, WoodResource, StoneResource, Building, 
    }
    
    /// <summary>
    /// Cells are the main structure of game world, they contain data of objects,
    /// they have types, dependent of their position.
    /// Base class for a cell in a grid map, need to be inherited
    /// </summary>
    public abstract class Cell
    {
        public CellContentType content;

        protected Cell()
        {
            content = CellContentType.Nothing;
        }
        

    }
}
