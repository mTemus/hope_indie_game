using Code.Utilities;
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
        private float width = 100f;
        private float height = 30f;

        void Start()
        {
            instance = this;
            
            int tileSize = GlobalProperties.TileSize;
            int widthTileCnt = (int) (width / tileSize);
            int heightTileCnt = (int) (height / tileSize);
        
            grid = new Grid.Grid(widthTileCnt, heightTileCnt, tileSize);

            if (showGrid) 
                __debug__CreateGridText();
        }

        void Update()
        {
            if (showGrid) 
                __debug__ShowGrid();
        }

        private void __debug__CreateGridText()
        {
            for (int x = 0; x < grid.Cells.GetLength(0); x++) {
                for (int y = 0; y < grid.Cells.GetLength(1); y++) {
                    CodeMonkeyUtils.ShowWorldText(x + "," + y, null, grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f, 8, Color.white,
                        TextAnchor.MiddleCenter);
                }
            }
        }
    
        private void __debug__ShowGrid()
        {
            for (int x = 0; x < grid.Cells.GetLength(0); x++) 
            for (int y = 0; y < grid.Cells.GetLength(1); y++) {
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.white);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.white);
            }
        
            Debug.DrawLine(grid.GetWorldPosition(0, grid.Height), grid.GetWorldPosition(grid.Width, grid.Height), Color.white);
            Debug.DrawLine(grid.GetWorldPosition(grid.Width, 0), grid.GetWorldPosition(grid.Width, grid.Height), Color.white);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 districtSize = new Vector3(width, height, 0);
            
            if (showDistrict) 
                Gizmos.DrawWireCube(transform.position + districtSize * 0.5f, districtSize);
        }

        public static Area Instance => instance;

        public Grid.Grid Grid => grid;
    }
}