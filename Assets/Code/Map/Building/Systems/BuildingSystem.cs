using System.Collections.Generic;
using Code.System;
using Code.System.Area;
using Code.System.Grid;
using Code.System.PlayerInput;
using Code.System.Properties;
using UnityEngine;

namespace Code.Map.Building.Systems
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private BuildingScript[] buildings;

        private int buildingIdx;
        private readonly int maxXOffset = 20;
        private Vector3Int currOffset;

        private static GameObject _currentBuilding;
        private static BuildingScript _currentBuildingData;
        
        public void ChangeBuilding(int value)
        {
            buildingIdx = Utilities.Utilities.IncrementIdx(buildingIdx, value, buildings.Length);
            SetBuilding(buildingIdx);
        }

        public void SetBuilding(int buildingId)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            //TODO: CLEAN THIS CODE
            Vector2 playerPos = Managers.Instance.Player.GetPlayerLocalPosition();
            Area playerArea = Managers.Instance.Areas.GetPlayerArea();
            playerArea.GridMap.GetXY(playerPos, out int x, out int y);
            _currentBuildingData = buildings[buildingId];

            Vector3Int buildingPosition = new Vector3Int(x, y, 0) * GlobalProperties.WorldTileSize;
            if (!playerArea.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.width)) 
                while (!playerArea.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.width)) 
                    buildingPosition.x -= 1 * GlobalProperties.WorldTileSize;
            
            buildingPosition /= GlobalProperties.WorldTileSize;
            
            _currentBuilding = Instantiate(
                _currentBuildingData.prefab.gameObject, 
                playerArea.GridMap.GetWorldPosition(buildingPosition.x, buildingPosition.y - 1, playerArea.transform.position), 
                Quaternion.identity, 
                playerArea.transform);
        }

        public void MoveCurrentBuilding(Vector3Int direction)
        {
            Vector3Int currBuildPos = Vector3Int.FloorToInt(_currentBuilding.transform.localPosition);
            direction *= GlobalProperties.WorldTileSize;
            
            if (Mathf.Abs(currOffset.x + direction.x) > maxXOffset) return;
            if (!Managers.Instance.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y)) return;
            if (!Managers.Instance.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y, _currentBuildingData.width)) return;
            
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
                playerArea.GridMap.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), new Vector2Int(buildingScript.width, buildingScript.height));
            
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
            _currentBuildingData = null;
            currOffset = Vector3Int.zero;
            
            Managers.Instance.Input.SetState(InputManager.MovingInputState);
        }

        public void CancelBuilding()
        {
            DestroyImmediate(_currentBuilding);
            
            _currentBuilding = null;
            _currentBuildingData = null;
            currOffset = Vector3Int.zero;
        }

        public static GameObject CurrentBuilding => _currentBuilding;
    }
}
