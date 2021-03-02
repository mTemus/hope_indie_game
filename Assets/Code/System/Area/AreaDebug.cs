using System.Collections.Generic;
using Code.Utilities;
using UnityEngine;

namespace Code.System.Area
{
    public class AreaDebug : MonoBehaviour
    {
        [SerializeField] private Area myArea;

        private List<TextMesh> gridText;

        public void CreateGridText()
        {
            GameObject textPool = new GameObject("DebugPool");
            textPool.transform.SetParent(transform);

            Grid.Grid grid = myArea.Grid;
            gridText = new List<TextMesh>();
            
            for (int x = 0; x < grid.Cells.GetLength(0); x++) {
                for (int y = 0; y < grid.Cells.GetLength(1); y++) {
                    gridText.Add(CodeMonkeyUtils.ShowWorldText(x + "," + y, textPool.transform, grid.GetWorldPosition(x, y, transform.position) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f, 8, Color.white,
                        TextAnchor.MiddleCenter));
                }
            }
        }
    
        public void ShowGrid()
        {
            Vector3Int areaPos = Vector3Int.FloorToInt(transform.position);
            Grid.Grid grid = myArea.Grid;
            
            for (int x = 0; x < grid.Cells.GetLength(0); x++) 
            for (int y = 0; y < grid.Cells.GetLength(1); y++) {
                Debug.DrawLine(grid.GetWorldPosition(x, y, areaPos), grid.GetWorldPosition( x, y + 1, areaPos), Color.white);
                Debug.DrawLine(grid.GetWorldPosition(x, y, areaPos), grid.GetWorldPosition(x + 1, y, areaPos), Color.white);
            }
        
            Debug.DrawLine(grid.GetWorldPosition(0, areaPos.y + grid.Height, areaPos), grid.GetWorldPosition(grid.Width, grid.Height, areaPos), Color.white);
            Debug.DrawLine(grid.GetWorldPosition(grid.Width, areaPos.y + 0, areaPos), grid.GetWorldPosition(grid.Width, grid.Height, areaPos), Color.white);
        }

        public void ToggleGridText(bool condition)
        {
            foreach (TextMesh text in gridText) 
                text.gameObject.SetActive(condition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 areaSize = new Vector3(myArea.Width, myArea.Height, 0);
            
            Gizmos.DrawWireCube(transform.position + areaSize * 0.5f, areaSize);
        }
        
    }
}
