using Code.System.Properties;
using UnityEngine;

namespace Code.System.Area
{
    public class Area : MonoBehaviour
    {
        [SerializeField] private bool showGrid = true;
        [SerializeField] private bool showDistrict = true;

        private static Area instance;
        
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