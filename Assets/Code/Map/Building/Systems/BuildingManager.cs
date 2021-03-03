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
            SetBuilding(buildingIdx);
        }

        public void SetBuilding(int buildingId)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);

            Vector2 playerPos = Managers.Instance.Player.GetPlayerLocalPosition();
            Area playerArea = Managers.Instance.Areas.GetPlayerArea();
            playerArea.GridMap.GetXY(playerPos, out int x, out int y);
            
            _currentBuilding = Instantiate(
                buildings[buildingId].prefab.gameObject, 
                playerArea.GridMap.GetWorldPosition(x, y - 1, playerArea.transform.position), 
                Quaternion.identity, 
                playerArea.transform);
        }

        public void MoveCurrentBuilding(Vector3Int direction)
        {
            Vector3Int currBuildPos = Vector3Int.FloorToInt(_currentBuilding.transform.localPosition);
            direction *= GlobalProperties.TileSize;

            if (Mathf.Abs(currOffset.x + direction.x) > maxXOffset) return;
            if (Managers.Instance.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y)) return;
            
            currOffset += direction;
            _currentBuilding.transform.localPosition += direction;
        }

        public void BuildBuilding()
        {
            Transform currBuildTransf = _currentBuilding.transform;
            Vector3Int currBuildPos = Vector3Int.FloorToInt(currBuildTransf.localPosition);
            Area playerArea = Managers.Instance.Areas.GetPlayerArea();
            
            BuildingScript buildingScript = buildings[buildingIdx];
            List<Vector2Int> buildingArea =
                playerArea.GridMap.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.TileSize, currBuildPos.y), new Vector2Int(buildingScript.width, buildingScript.height));
            
            foreach (Vector2Int tilePos in buildingArea) {
                Cell cell = playerArea.GridMap.GetCellAt(tilePos.x, tilePos.y);
            
                if (cell.CanBuild()) continue;
                Debug.LogWarning("Can't build this object at: " + tilePos.x + " " + tilePos.y);
                return;
            }
            
            foreach (Vector2Int tilePos in buildingArea) {
                Cell cell = playerArea.GridMap.GetCellAt(tilePos.x, tilePos.y);
                cell.SetBuildingAtCell(currBuildTransf);
                cell.SetBuildingScriptAtCell(buildingScript);
            }
            
            _currentBuilding = null;
            currOffset = Vector3Int.zero;
            
            Managers.Instance.Input.SetState(InputManager.MovingInputState);
        }

        public static GameObject CurrentBuilding => _currentBuilding;
    }
}
