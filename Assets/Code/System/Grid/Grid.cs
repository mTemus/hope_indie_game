using System.Collections.Generic;
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
        
        public List<Vector2Int> GetTileWithNeighbours(Vector2Int tile, Vector2Int objectSize)
        {
            List<Vector2Int> tiles = new List<Vector2Int>();

            for (int x = 0; x < objectSize.x; x++) {
                for (int y = 0; y < objectSize.y; y++) {
                    tiles.Add(new Vector2Int(tile.x + x, tile.y + y));
                }
            }
            return tiles;
        }

        public Vector3 GetWorldPosition(int x, int y, Vector3 worldAreaPos) => 
            new Vector3(x, y) * cellSize + worldAreaPos;
        
        public Vector3 GetLocalAreaPosition(int x, int y) => 
            new Vector3(x, y) * cellSize;
        
        public Vector2 GetWorldPosition(Vector2 pos) => 
            pos * cellSize;

        public Cell GetCellAt(int x, int y) =>
            cells[x, y];

        public bool IsTileInRange(int x, int y) =>
            y <= height && y >= 0 && x <= width && x >= 0;

        public int CellSize => cellSize;

        public Cell[,] Cells => cells;

        public int Width => width;

        public int Height => height;
    }
    
    
    
}
