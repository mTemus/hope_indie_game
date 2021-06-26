using System;

namespace Code.System.Grid
{
    public enum SurfaceCellType
    {
        EmptySpace, WoodResource, StoneResource, Building, 
    }
    
    public class SurfaceCell : Cell
    {
        private SurfaceCellType type = SurfaceCellType.EmptySpace;
        private SurfaceCellType previousSurface;

        public SurfaceCellType Type => type;

        public SurfaceCellType PreviousSurface => previousSurface;

        public void SetNewSurface(SurfaceCellType newType)
        {
            previousSurface = type;
            type = newType;
        }
    }
}
