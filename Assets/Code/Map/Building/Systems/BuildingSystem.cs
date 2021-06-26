using System.Collections.Generic;
using System.Linq;
using Code.System;
using Code.System.Areas;
using Code.System.GameInput;
using Code.System.Properties;
using UnityEngine;

namespace Code.Map.Building.Systems
{
    //TODO: Apply systems dependent on building type/building resources, when no resources, then set resources delivered in building task
    
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingFadeMaterial;
        
        private readonly int maxXOffset = 20;
        private Vector3Int currOffset;

        private static GameObject _currentBuilding;
        private static BuildingData _currentBuildingData;
        
        public void SetBuilding(BuildingData buildingData)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            //TODO: CLEAN THIS CODE
            Vector2 playerPos = Managers.I.Player.GetPlayerLocalPosition();
            Area playerArea = Managers.I.Areas.GetPlayerArea();
            playerArea.GridMap.GetXY(playerPos, out int x, out int y);
            _currentBuildingData = buildingData;

            Vector3Int buildingPosition = new Vector3Int(x, y, 0) * GlobalProperties.WorldTileSize;
            
            if (!IsBuildingInAreaRange(currentBuildingArea, buildingPosition)) 
                while (!IsBuildingInAreaRange(currentBuildingArea, buildingPosition))
                    if (buildingPosition.x < 0) 
                        buildingPosition.x += 1 * GlobalProperties.WorldTileSize;
                    else if (buildingPosition.x > 0)
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
            if (!Managers.I.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y)) return;
            if (!Managers.I.Areas.GetPlayerArea().GridMap.IsTileInRange(currBuildPos.x + direction.x, currBuildPos.y, _currentBuildingData.Size.x)) return;
            
            currOffset += direction;
            _currentBuilding.transform.localPosition += direction;
        }

        public void BuildBuilding()
        {
           if ((from requiredResource 
               in _currentBuildingData.RequiredResources 
               let currentResource = Managers.I.Resources.GetResourceByType(requiredResource.Type) 
               where currentResource.amount < requiredResource.amount 
               select requiredResource)
               .Any()) return; 

            Transform currBuildTransform = _currentBuilding.transform;
            Vector3Int currBuildPos = Vector3Int.FloorToInt(currBuildTransform.localPosition);
            Area playerArea = Managers.I.Areas.GetPlayerArea();
            
            List<Vector2Int> buildingArea =
                playerArea.GridMap.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), new Vector2Int(_currentBuildingData.Size.x, _currentBuildingData.Size.y));

            if (!playerArea.CanPlaceBuilding(buildingArea)) {
                Debug.LogWarning("Can't build this object there");
                return;
            }
            
            _currentBuilding.GetComponent<Construction>()
                .InitializeConstruction(_currentBuildingData, new Material(buildingFadeMaterial));
            playerArea.AddBuilding(_currentBuilding.GetComponent<Building>(), _currentBuildingData);
            
            _currentBuilding = null;
            _currentBuildingData = null;
            currOffset = Vector3Int.zero;
            
            Managers.I.Input.SetState(InputManager.MovingInputState);
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
