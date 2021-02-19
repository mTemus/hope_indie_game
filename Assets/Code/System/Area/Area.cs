using Code.Utilities;
using UnityEngine;

namespace Code.System.Area
{
    public class Area : MonoBehaviour
    {
        [SerializeField] private bool showGrid = true;
        [SerializeField] private bool showDistrict = true;

        private Grid.Grid _grid;
        private float width = 100f;
        private float height = 50f;

        void Start()
        {
            int tileSize = 2;
            int widthTileCnt = (int) (width / tileSize);
            int heightTileCnt = (int) (height / tileSize);
        
            _grid = new Grid.Grid(widthTileCnt, heightTileCnt, tileSize);

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
            for (int x = 0; x < _grid.Tiles.GetLength(0); x++) {
                for (int y = 0; y < _grid.Tiles.GetLength(1); y++) {
                    CodeMonkeyUtils.ShowWorldText(x + "," + y, null, _grid.GetWorldPosition(x, y) + new Vector3(_grid.TileSize, _grid.TileSize) * 0.5f, 8, Color.white,
                        TextAnchor.MiddleCenter);
                }
            }
        }
    
        private void __debug__ShowGrid()
        {
            for (int x = 0; x < _grid.Tiles.GetLength(0); x++) 
            for (int y = 0; y < _grid.Tiles.GetLength(1); y++) {
                Debug.DrawLine(_grid.GetWorldPosition(x, y), _grid.GetWorldPosition(x, y + 1), Color.white);
                Debug.DrawLine(_grid.GetWorldPosition(x, y), _grid.GetWorldPosition(x + 1, y), Color.white);
            }
        
            Debug.DrawLine(_grid.GetWorldPosition(0, _grid.Height), _grid.GetWorldPosition(_grid.Width, _grid.Height), Color.white);
            Debug.DrawLine(_grid.GetWorldPosition(_grid.Width, 0), _grid.GetWorldPosition(_grid.Width, _grid.Height), Color.white);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 districtSize = new Vector3(width, height, 0);
            
            if (showDistrict) 
                Gizmos.DrawWireCube(transform.position + districtSize * 0.5f, districtSize);
        }

    }
}