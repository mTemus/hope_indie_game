using Code.System.Properties;
using UnityEngine;

namespace Code.System.Area
{
    public enum AreaType
    {
        VILLAGE,
        FOREST,
        EMPTY,
    }

    public abstract class Area : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private AreaType type;
        [SerializeField] private float width = 100f;
        [SerializeField] private float height = 30f;
        
        private Grid.Grid grid;
        
        void Awake()
        {
            int widthTileCnt = (int) (width / GlobalProperties.TileSize);
            int heightTileCnt = (int) (height / GlobalProperties.TileSize);
        
            grid = new Grid.Grid(widthTileCnt, heightTileCnt, GlobalProperties.TileSize);
        }

        public Grid.Grid Grid => grid;

        public AreaType Type => type;

        public float Width => width;

        public float Height => height;
    }
}