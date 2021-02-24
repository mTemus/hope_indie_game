using UnityEngine;

namespace Code.System.Grid
{
    public class Grid
    {
        private readonly int width;
        private readonly int height;
        private readonly int cellSize;

        private readonly Cell[,] cells;

        public Grid(int width, int height, int cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            cells = new Cell[width, height];

            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++) 
                cells[i, j] = new Cell();
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.y / cellSize);
        }

        public Vector3 GetWorldPosition(int x, int y) => 
            new Vector3(x, y) * cellSize;
        
        public Vector2 GetWorldPosition(Vector2 pos) => 
            pos * cellSize;

        public Cell GetCellAt(int x, int y) =>
            cells[x, y];

        public int CellSize => cellSize;

        public Cell[,] Cells => cells;

        public int Width => width;

        public int Height => height;
    }
    
    
    
}
