using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.GameProperties;
using HopeMain.Code.World.Buildings;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.World.Grid
{
    public class GridMap
    {
        private readonly int width;
        private readonly int height;
        private readonly int cellSize;

        private readonly Cell[,] cells;

        public GridMap(int width, int height, int cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            cells = new Cell[width, height];

            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (j == 0) cells[i, j] = new SurfaceCell();
                else cells[i, j] = new SpaceCell();
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

        public bool IsTileInRange(int x, int y)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y <= height && y >= 0 && x <= width && x >= 0;
        }
        
        public bool IsTileInRange(int x, int y, int objectWidth)
        {
            x /= GlobalProperties.WorldTileSize;
            y /= GlobalProperties.WorldTileSize;
            return y  <= height && y >= 0 && x + objectWidth <= width && x >= 0;
        }

        public void SetBuildingInGrid(List<Vector2Int> area, Building building)
        {
            foreach (var cell in area
                .Select(cellPos => GetCellAt(cellPos.x, cellPos.y))) {
                cell.buildingData = building;
                cell.content = CellContentType.Building;
            }
        }

        public void SetResourceToGatherInGrid(List<Vector2Int> area, ResourceToGather resourceToGather)
        {
            foreach (var cell in area
                .Select(cellPos => GetCellAt(cellPos.x, cellPos.y))) {
                cell.resourceToGatherData = resourceToGather;

                switch (resourceToGather.Resource.Type) {
                    case ResourceType.WOOD:
                        cell.content = CellContentType.WoodResource;
                        break;
                    
                    case ResourceType.STONE:
                        cell.content = CellContentType.StoneResource;
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public int CellSize => cellSize;

        public Cell[,] Cells => cells;

        public int Width => width;

        public int Height => height;
    }
}
