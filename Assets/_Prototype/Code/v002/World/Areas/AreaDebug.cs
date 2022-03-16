using System.Collections.Generic;
using _Prototype.Code.v001.Utilities;
using _Prototype.Code.v002.World.Grid;
using TMPro;
using UnityEngine;

namespace _Prototype.Code.v002.World.Areas
{
    /// <summary>
    /// Class containing data used to debug Area object
    /// </summary>
    public class AreaDebug : MonoBehaviour
    {
        [SerializeField] private Area myArea;

        private List<TextMeshPro> _gridText;

        /// <summary>
        /// Create gizmos and text to illustrate grid on GridMap object
        /// </summary>
        public void CreateGridText()
        {
            GameObject textPool = new GameObject("DebugPool");
            textPool.transform.SetParent(transform);

            GridMap gridMap = myArea.GridMap;
            _gridText = new List<TextMeshPro>();
            
            for (int x = 0; x < gridMap.Cells.GetLength(0); x++) {
                for (int y = 0; y < gridMap.Cells.GetLength(1); y++) {
                    _gridText.Add(
                        CodeMonkeyUtils.ShowWorldText(
                            x + "," + y, textPool.transform, 
                        gridMap.GetWorldPosition(
                            x, y, transform.position) + new Vector3(
                            gridMap.CellSize, 
                            gridMap.CellSize) * 0.5f, 
                        3, 
                        Color.white
                            ));
                }
            }
        }
    
        /// <summary>
        /// Show grid gizmos
        /// </summary>
        public void ShowGrid()
        {
            Vector3Int areaPos = Vector3Int.FloorToInt(transform.position);
            GridMap gridMap = myArea.GridMap;
            
            for (int x = 0; x < gridMap.Cells.GetLength(0); x++) 
            for (int y = 0; y < gridMap.Cells.GetLength(1); y++) {
                Debug.DrawLine(gridMap.GetWorldPosition(x, y, areaPos), gridMap.GetWorldPosition( x, y + 1, areaPos), Color.white);
                Debug.DrawLine(gridMap.GetWorldPosition(x, y, areaPos), gridMap.GetWorldPosition(x + 1, y, areaPos), Color.white);
            }
        
            Debug.DrawLine(gridMap.GetWorldPosition(0, areaPos.y + gridMap.Height, areaPos), gridMap.GetWorldPosition(gridMap.Width, gridMap.Height, areaPos), Color.white);
            Debug.DrawLine(gridMap.GetWorldPosition(gridMap.Width, areaPos.y + 0, areaPos), gridMap.GetWorldPosition(gridMap.Width, gridMap.Height, areaPos), Color.white);
            Debug.Log("here");
        }

        /// <summary>
        /// Show grid text
        /// </summary>
        /// <param name="condition">True - show text, False - hide text</param>
        public void ToggleGridText(bool condition)
        {
            foreach (TextMeshPro text in _gridText) 
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
