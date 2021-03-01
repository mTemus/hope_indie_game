using System.Collections.Generic;
using Code.Player;
using Code.System;
using Code.System.Area;
using Code.System.Grid;
using Code.System.PlayerInput;
using Code.System.Properties;
using UnityEngine;

namespace Code.Map.Building.Systems
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingScript[] buildings;

        private int buildingIdx;
        private readonly int maxXOffset = 20;
        private Vector3Int currOffset;

        private static GameObject _currentBuilding;
        
        public void ChangeBuilding(int value)
        {
            buildingIdx = Utilities.Utilities.IncrementIdx(buildingIdx, value, buildings.Length);
            
            Debug.Log(buildingIdx);
            SetBuilding(buildingIdx);
        }

        public void SetBuilding(int buildingId)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            Vector2 playerPos = PlayerManager.Instance.GetPlayerPosition();

            Area.Instance.Grid.GetXY(playerPos, out int x, out int y);
            BuildingScript currBuildingTempl = buildings[buildingId];
            _currentBuilding = Instantiate(currBuildingTempl.prefab.gameObject, Area.Instance.Grid.GetWorldPosition(x, y - 1), Quaternion.identity);
        }

        public void MoveCurrentBuilding(Vector3Int direction)
        {
            Vector3Int currBuildPos = Vector3Int.FloorToInt(_currentBuilding.transform.position);
            direction *= GlobalProperties.TileSize;

            if (Mathf.Abs(currOffset.x + direction.x) > maxXOffset) return;
            if (!Area.Instance.Grid.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y)) return;
            
            currOffset += direction;
            _currentBuilding.transform.position += direction;
        }

        public void BuildBuilding()
        {
            Transform currBuildTransf = _currentBuilding.transform;
            Vector3Int currBuildPos = Vector3Int.FloorToInt(currBuildTransf.position);

            BuildingScript buildingScript = buildings[buildingIdx];
            List<Vector2Int> buildingArea =
                Area.Instance.Grid.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.TileSize, currBuildPos.y), new Vector2Int(buildingScript.width, buildingScript.height));
            
            Debug.Log("here");
            
            foreach (Vector2Int tilePos in buildingArea) {
                Cell cell = Area.Instance.Grid.GetCellAt(tilePos.x, tilePos.y);

                if (cell.CanBuild()) continue;
                Debug.LogWarning("Can't build this object at: " + tilePos.x + " " + tilePos.y);
                return;
            }

            Debug.Log("here2 " + buildingArea.Count);
            
            foreach (Vector2Int tilePos in buildingArea) {
                Debug.Log(tilePos);
                
                Cell cell = Area.Instance.Grid.GetCellAt(tilePos.x, tilePos.y);
                cell.SetBuildingAtCell(currBuildTransf);
                cell.SetBuildingScriptAtCell(buildingScript);
            }

            Debug.Log("here3");
            
            _currentBuilding = null;
            currOffset = Vector3Int.zero;
            
            Managers.Instance.Input.SetState(InputManager.MovingInputState);
        }

        public static GameObject CurrentBuilding => _currentBuilding;
    }
}
