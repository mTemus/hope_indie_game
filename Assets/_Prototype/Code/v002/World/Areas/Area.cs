using _Prototype.Code.v001.GameProperties;
using _Prototype.Code.v002.World.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Prototype.Code.v002.World.Areas
{
    /// <summary>
    /// Describing type/content of area
    /// </summary>
    public enum AreaType
    {
        Village,
        Forest,
        Highlands,
        Empty,
    }

    /// <summary>
    /// Class containing whole data of an area in game world
    /// </summary>
    public class Area : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private AreaType type;
        [SerializeField] private int width = 100;
        [SerializeField] private int height = 30;

        [Header("Environment")] 
        [SerializeField] private Tilemap _tilemap;
        
        private GameObject _playerObject;
        private GridMap _gridMap;
        
        public GridMap GridMap => _gridMap;
        public AreaType Type => type;
        public int Width => width;
        public int Height => height;
        public Vector3 AreaWorldPosition => transform.position;

        private void Awake()
        {
            _gridMap = new GridMap(
                width / GlobalProperties.WorldTileSize, 
                height / GlobalProperties.WorldTileSize, 
                GlobalProperties.WorldTileSize, 
                _tilemap
                );
        }
    }
}