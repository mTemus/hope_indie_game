using System;
using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.GameProperties;
using HopeMain.Code.System;
using HopeMain.Code.System.GameInput;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Grid;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.World.Buildings.Systems
{
    //TODO: Apply systems dependent on building type/building resources, when no resources, then set resources delivered in building task
    
    // NOTE: Building system can be optimized by checking/setting only cell content type in surface cell
    
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingFadeMaterial;
        
        private readonly int maxXOffset = 30;
        private Vector3Int currOffset;

        private static GameObject _currentBuilding;
        private static BuildingData _currentBuildingData;
        private Area currentBuildingArea;
        private Vector3Int currentPlacingPosition;
        
        // 1. Set building -> set building chosen from building UI and instantiate it on player position
            // after setting Move(Vector.zero)
        // 2. Move -> Prepare new building position based on direction given by player
        // 3. CheckPosition -> check new building position for surface type, and place building according to its requirements
        // 4. Build -> set building on current position if it is valid for this type of building or it is free
        
        public static GameObject CurrentBuilding => _currentBuilding;
        public static BuildingData CurrentBuildingData => _currentBuildingData;
        
        private bool IsBuildingInAreaRange(Area area, Vector3Int buildingPosition) =>
            area.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.Size.x);
        
        private bool CheckBuildingCorner(Vector3Int cornerPosition, out Vector3 newBuildingPosition)
        {
            Cell cell = currentBuildingArea.GridMap.GetCellAt(cornerPosition.x / GlobalProperties.WorldTileSize, 0);
            if (cell.content == _currentBuildingData.RequiredCellContent) {
                switch (cell.content) {
                    case CellContentType.Null:
                        break;
                    
                    case CellContentType.Nothing:
                        newBuildingPosition = cornerPosition;
                        return true;
                        
                    case CellContentType.WoodResource:
                        break;
                    
                    case CellContentType.StoneResource:
                        ResourceToGather resource = cell.resourceToGatherData;
                        newBuildingPosition = new Vector3(
                            (int) resource.transform.localPosition.x + Mathf.FloorToInt(_currentBuildingData.Size.x / 2f) * GlobalProperties.WorldTileSize, 
                            cornerPosition.y, 
                            cornerPosition.z);
                        return true;
                    
                    case CellContentType.Building:
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            newBuildingPosition = Vector3.zero;
            return false;
        }

        private Vector3 CheckBuildingCurrentPosition()
        {
            Vector3 newBuildingPosition = Vector3.zero;

            switch (_currentBuildingData.RequiredCellContent) {
                case CellContentType.Null:
                    break;
                
                case CellContentType.Nothing:
                    CheckBuildingCorner(currentPlacingPosition, out newBuildingPosition);
                    break;
                
                case CellContentType.WoodResource:
                    break;
                
                case CellContentType.StoneResource:
                    if (!CheckBuildingCorner(currentPlacingPosition, out newBuildingPosition)) 
                        CheckBuildingCorner( 
                            Vector3Int.FloorToInt(new Vector3(
                                currentPlacingPosition.x + _currentBuildingData.Size.x * GlobalProperties.WorldTileSize, 
                                currentPlacingPosition.y, 
                                currentPlacingPosition.z)),
                            out newBuildingPosition);
                    break;
                
                case CellContentType.Building:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            return newBuildingPosition == Vector3.zero ? currentPlacingPosition : newBuildingPosition;
        }
        
        public void SetBuilding(BuildingData buildingData)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            _currentBuildingData = buildingData;
            currentBuildingArea = Managers.I.Areas.GetPlayerArea();
            currentBuildingArea.GridMap.GetXY(Managers.I.Player.GetPlayerLocalPosition(), out int x, out int y);

            Vector3Int buildingPosition = new Vector3Int(x, 0, 0) * GlobalProperties.WorldTileSize;
            
            if (!IsBuildingInAreaRange(currentBuildingArea, buildingPosition)) 
                while (!IsBuildingInAreaRange(currentBuildingArea, buildingPosition))
                    if (buildingPosition.x < 0) buildingPosition.x += 1 * GlobalProperties.WorldTileSize;
                    else if (buildingPosition.x > 0) buildingPosition.x -= 1 * GlobalProperties.WorldTileSize;

            buildingPosition /= GlobalProperties.WorldTileSize;
            
            _currentBuilding = Instantiate(
                _currentBuildingData.Prefab.gameObject, 
                currentBuildingArea.GridMap.GetWorldPosition(buildingPosition.x, buildingPosition.y, currentBuildingArea.transform.position), 
                Quaternion.identity, 
                currentBuildingArea.transform);
            
            currentPlacingPosition = Vector3Int.FloorToInt(buildingPosition) * GlobalProperties.WorldTileSize;
        }

        public void MoveCurrentBuilding(Vector3Int direction)
        {
            direction *= GlobalProperties.WorldTileSize;
            int newBuildPosX = currentPlacingPosition.x + direction.x;
            
            if (Mathf.Abs(currOffset.x + direction.x) > maxXOffset) return;
            if (!currentBuildingArea.GridMap.IsTileInRange(newBuildPosX, currentPlacingPosition.y)) return;
            if (!currentBuildingArea.GridMap.IsTileInRange(newBuildPosX, currentPlacingPosition.y, _currentBuildingData.Size.x)) return;
            
            currOffset += direction;
            currentPlacingPosition += direction;
            
            _currentBuilding.transform.localPosition = CheckBuildingCurrentPosition();
        }

        public void BuildBuilding()
        {
            if ((from requiredResource in _currentBuildingData.RequiredResources
                    let currentResource = Managers.I.Resources.GetResourceByType(requiredResource.Type)
                    where currentResource.amount < requiredResource.amount
                    select requiredResource)
                .Any()) {
                Debug.LogError("Not enough resources to build.");
                return;
            }
            
            List<Vector2Int> buildingArea =
                currentBuildingArea.GridMap.GetTileWithNeighbours(
                    Vector2Int.FloorToInt(_currentBuilding.transform.localPosition) / GlobalProperties.WorldTileSize, 
                    _currentBuildingData.Size);
            
            if (!currentBuildingArea.CanPlaceBuilding(buildingArea, _currentBuildingData.RequiredCellContent)) {
                Debug.LogWarning("Can't build this object there");
                return;
            }
            
            _currentBuilding.GetComponent<Construction>()
                .InitializeConstruction(_currentBuildingData, new Material(buildingFadeMaterial));
            currentBuildingArea.AddBuilding(_currentBuilding.GetComponent<Building>(), _currentBuildingData);

            switch (_currentBuildingData.RequiredCellContent) {
                case CellContentType.Null:
                    break;
                
                case CellContentType.Nothing:
                    break;
                
                case CellContentType.WoodResource:
                    break;
                
                case CellContentType.StoneResource:
                    if (currentBuildingArea.GridMap.GetCellAt(buildingArea[0].x, buildingArea[0].y).resourceToGatherData
                        is StoneToGather stone)
                        stone.Workplace = _currentBuilding.GetComponent<Workplace.Workplace>();
                    break;
                
                case CellContentType.Building:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentBuilding = null;
            _currentBuildingData = null;
            currentBuildingArea = null;
            currentPlacingPosition = Vector3Int.zero;
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

        
    }
}
