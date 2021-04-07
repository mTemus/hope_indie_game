using System.Collections.Generic;
using System.Linq;
using Code.System;
using Code.System.Area;
using Code.System.PlayerInput;
using Code.System.Properties;
using UnityEngine;

namespace Code.Map.Building.Systems
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingFadeMaterial;
        
        private readonly int maxXOffset = 20;
        private Vector3Int currOffset;

        private static GameObject _currentBuilding;
        private static BuildingData _currentBuildingData;
        private static WorkplaceProperties _currentWorkplaceProperties;
        
        public void SetBuilding(BuildingData buildingData, WorkplaceProperties workplaceProperties)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            //TODO: CLEAN THIS CODE
            Vector2 playerPos = Managers.Instance.Player.GetPlayerLocalPosition();
            Area playerArea = Managers.Instance.Areas.GetPlayerArea();
            playerArea.GridMap.GetXY(playerPos, out int x, out int y);
            _currentBuildingData = buildingData;
            _currentWorkplaceProperties = workplaceProperties;

            Vector3Int buildingPosition = new Vector3Int(x, y, 0) * GlobalProperties.WorldTileSize;
            if (!playerArea.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.Size.x)) 
                while (!playerArea.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.Size.x)) 
                    buildingPosition.x -= 1 * GlobalProperties.WorldTileSize;
            
            buildingPosition /= GlobalProperties.WorldTileSize;
            
            _currentBuilding = Instantiate(
                _currentBuildingData.Prefab.gameObject, 
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
            if (!Managers.Instance.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y, _currentBuildingData.Size.x)) return;
            
            currOffset += direction;
            _currentBuilding.transform.localPosition += direction;
        }

        public void BuildBuilding()
        {
            Transform currBuildTransf = _currentBuilding.transform;
            Vector3Int currBuildPos = Vector3Int.FloorToInt(currBuildTransf.localPosition);
            Area playerArea = Managers.Instance.Areas.GetPlayerArea();
            
            List<Vector2Int> buildingArea =
                playerArea.GridMap.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), new Vector2Int(_currentBuildingData.Size.x, _currentBuildingData.Size.y));

            if (!playerArea.CanPlaceBuilding(buildingArea)) {
                Debug.LogWarning("Can't build this object there");
                return;
            }
            
            playerArea.FillTiles(buildingArea, currBuildTransf);
            
            _currentBuilding.GetComponent<Construction>().InitializeConstruction(_currentBuildingData, new Material(buildingFadeMaterial));
            playerArea.AddBuilding(_currentBuilding.GetComponent<Building>());

            if (_currentWorkplaceProperties != null) {
                _currentBuilding.GetComponent<Workplace>().Properties = _currentWorkplaceProperties;
                _currentWorkplaceProperties = null;
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

        public static BuildingData CurrentBuildingData => _currentBuildingData;
    }
}
