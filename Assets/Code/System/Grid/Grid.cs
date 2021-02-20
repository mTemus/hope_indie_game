using UnityEngine;

namespace Code.System.Grid
{
    public class Grid
    {
        private int _width;
        private int _height;
        private readonly int _tileSize;

        public Tile[,] tiles;

        public Grid(int width, int height, int tileSize)
        {
            _width = width;
            _height = height;
            _tileSize = tileSize;

            tiles = new Tile[width, height];

            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++) 
                tiles[i, j] = new Tile();
        }

        public Vector3 GetWorldPosition(int x, int y) => 
            new Vector3(x, y) * _tileSize;

        public int TileSize => _tileSize;

        public Tile[,] Tiles => tiles;

        public int Width => _width;

        public int Height => _height;
    }
    
    
    
}
