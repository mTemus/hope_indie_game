using System;
using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.GameProperties;
using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Grid.Cell;
using _Prototype.Code.v001.World.Resources;
using _Prototype.Code.v001.World.Resources.ResourceToGather;
using UnityEngine;

namespace _Prototype.Code.v001.World.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class GridMap
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;

        private readonly CellBase[,] _cells;

        public GridMap(int width, int height, int cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _cells = new CellBase[width, height];

            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (j == 0) _cells[i, j] = new SurfaceCell();
                else _cells[i, j] = new SpaceCell();
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
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="objectSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="worldAreaPos"></param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y, Vector3 worldAreaPos) => 
            new Vector3(x, y) * _cellSize + worldAreaPos;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 GetLocalAreaPosition(int x, int y) => 
            new Vector3(x, y) * _cellSize;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 GetWorldPosition(Vector2 pos) => 
            pos * _cellSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public CellBase GetCellAt(int x, int y) =>
            _cells[x, y];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsTileInRange(int x, int y)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y <= _height && y >= 0 && x <= _width && x >= 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="objectWidth"></param>
        /// <returns></returns>
        public bool IsTileInRange(int x, int y, int objectWidth)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y  <= _height && y >= 0 && x + objectWidth <= _width && x >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="building"></param>
        public void SetBuildingInGrid(List<Vector2Int> area, Building building)
        {
            foreach (var cell in area
                .Select(cellPos => GetCellAt(cellPos.x, cellPos.y))) {
                cell.buildingData = building;
                cell.content = CellContentType.Building;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="resourceToGather"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetResourceToGatherInGrid(List<Vector2Int> area, ResourceToGatherBase resourceToGather)
        {
            foreach (var cell in area
                .Select(cellPos => GetCellAt(cellPos.x, cellPos.y))) {
                cell.resourceToGatherData = resourceToGather;

                switch (resourceToGather.Resource.Type) {
                    case ResourceType.Wood:
                        cell.content = CellContentType.WoodResource;
                        break;
                    
                    case ResourceType.Stone:
                        cell.content = CellContentType.StoneResource;
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public int CellSize => _cellSize;

        public CellBase[,] Cells => _cells;

        public int Width => _width;

        public int Height => _height;
    }
}
