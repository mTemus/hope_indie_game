using System.Collections.Generic;
using _Prototype.Code.v001.GameProperties;
using _Prototype.Code.v002.World.Grid.Cells;
using UnityEngine;
using SpaceCell = _Prototype.Code.v002.World.Grid.Cells.SpaceCell;

namespace _Prototype.Code.v002.World.Grid
{
    /// <summary>
    /// GridMap is an grid object containing cells. 
    /// </summary>
    public class GridMap
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;

        private readonly Cell[,] _cells;

        public GridMap(int width, int height, int cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (y == 0) _cells[x, y] = new LandCell();
                else _cells[x, y] = new SpaceCell();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            y = Mathf.FloorToInt(worldPosition.y / _cellSize);
        }
        
        /// <summary>
        /// Get cells around point
        /// </summary>
        /// <param name="tile">Point coordinates</param>
        /// <param name="objectSize">How many tiles around point should be returned</param>
        /// <returns>List of Cells</returns>
        public List<Vector2Int> GetCellWithNeighbours(Vector2Int tile, Vector2Int objectSize)
        {
            List<Vector2Int> cells = new List<Vector2Int>();

            for (int x = 0; x < objectSize.x; x++) {
                for (int y = 0; y < objectSize.y; y++) {
                    cells.Add(new Vector2Int(tile.x + x, tile.y + y));
                }
            }
            return cells;
        }

        /// <summary>
        /// Convert cells coordinates to world position
        /// </summary>
        /// <param name="x">Cell x</param>
        /// <param name="y">Cell y</param>
        /// <param name="worldAreaPos">Position of area Game Object in the game world</param>
        /// <returns>Cell position converted to world position</returns>
        public Vector3 GetWorldPosition(int x, int y, Vector3 worldAreaPos) => 
            new Vector3(x, y) * _cellSize + worldAreaPos;
        
        /// <summary>
        /// Convert cells coordinates to local position
        /// </summary>
        /// <param name="x">Cell x</param>
        /// <param name="y">Cell y</param>
        /// <returns>Cell position converted to local position</returns>
        public Vector3 GetLocalPosition(int x, int y) => 
            new Vector3(x, y) * _cellSize;
        
        /// <summary>
        /// Convert cells coordinates to world position
        /// </summary>
        /// <param name="pos">Cell position in vector2 (x/y)</param>
        /// <returns>Cell position converted to world position</returns>
        public Vector2 GetWorldPosition(Vector2 pos) => 
            pos * _cellSize;

        /// <summary>
        /// Returns object of cell at given coordinates
        /// </summary>
        /// <param name="x">X cell coord</param>
        /// <param name="y">Y cell coord</param>
        /// <returns>Cell</returns>
        public Cell GetCellAt(int x, int y) =>
            _cells[x, y];

        /// <summary>
        /// Checks if cell of given coordinates exists in grid map
        /// </summary>
        /// <param name="x">X cell coord</param>
        /// <param name="y">Y cell coord</param>
        /// <returns>True if cell is in grid map bounds or false if isn't</returns>
        public bool IsCellInBoundary(int x, int y)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y <= _height && y >= 0 && x <= _width && x >= 0;
        }
        
        /// <summary>
        /// Checks if cell of given coordinates exists in grid map
        /// </summary>
        /// <param name="x">X cell coord</param>
        /// <param name="y">Y cell coord</param>
        /// <param name="objectWidth">Number of cells that should be checked</param>
        /// <returns>True if all cells are in grid map bounds or false if any of cells isn't</returns>
        public bool IsCellInBoundary(int x, int y, int objectWidth)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y  <= _height && y >= 0 && x + objectWidth <= _width && x >= 0;
        }

        public int CellSize => _cellSize;

        public Cell[,] Cells => _cells;

        public int Width => _width;

        public int Height => _height;
    }
}
