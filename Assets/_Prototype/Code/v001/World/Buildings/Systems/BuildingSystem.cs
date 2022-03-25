using System;
using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.GameProperties;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.World.Areas;
using _Prototype.Code.v001.World.Grid.Cell;
using _Prototype.Code.v001.World.Resources.ResourceToGather;
using _Prototype.Code.v002.System.GameInput;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings.Systems
{
    //TODO: Apply systems dependent on building type/building resources, when no resources, then set resources delivered in building task
    
    // NOTE: Building system can be optimized by checking/setting only cell content type in surface cell
    
    /// <summary>
    /// 
    /// </summary>
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingFadeMaterial;
        
        private readonly int _maxXOffset = 30;
        private Vector3Int _currOffset;

        private static GameObject _currentBuilding;
        private static Data _currentBuildingData;
        private Area _currentBuildingArea;
        private Vector3Int _currentPlacingPosition;
        
        // 1. Set building -> set building chosen from building UI and instantiate it on player position
            // after setting Move(Vector.zero)
        // 2. Move -> Prepare new building position based on direction given by player
        // 3. CheckPosition -> check new building position for surface type, and place building according to its requirements
        // 4. Build -> set building on current position if it is valid for this type of building or it is free
        
        public static GameObject CurrentBuilding => _currentBuilding;
        public static Data CurrentBuildingData => _currentBuildingData;
        
        private bool IsBuildingInAreaRange(Area area, Vector3Int buildingPosition) =>
            area.GridMap.IsTileInRange(buildingPosition.x, buildingPosition.y, _currentBuildingData.Size.x);
        
        private bool CheckBuildingCorner(Vector3Int cornerPosition, out Vector3 newBuildingPosition)
        {
            CellBase cell = _currentBuildingArea.GridMap.GetCellAt(cornerPosition.x / GlobalProperties.WorldTileSize, 0);
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
                        ResourceToGatherBase resource = cell.resourceToGatherData;
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
                    CheckBuildingCorner(_currentPlacingPosition, out newBuildingPosition);
                    break;
                
                case CellContentType.WoodResource:
                    break;
                
                case CellContentType.StoneResource:
                    if (!CheckBuildingCorner(_currentPlacingPosition, out newBuildingPosition)) 
                        CheckBuildingCorner( 
                            Vector3Int.FloorToInt(new Vector3(
                                _currentPlacingPosition.x + _currentBuildingData.Size.x * GlobalProperties.WorldTileSize, 
                                _currentPlacingPosition.y, 
                                _currentPlacingPosition.z)),
                            out newBuildingPosition);
                    break;
                
                case CellContentType.Building:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            return newBuildingPosition == Vector3.zero ? _currentPlacingPosition : newBuildingPosition;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingData"></param>
        public void SetBuilding(Data buildingData)
        {
            if (_currentBuilding != null) 
                DestroyImmediate(_currentBuilding.gameObject);
            
            _currentBuildingData = buildingData;
            _currentBuildingArea = Managers.I.Areas.GetPlayerArea();
            _currentBuildingArea.GridMap.GetXY(Managers.I.Player.GetPlayerLocalPosition(), out int x, out int y);

            Vector3Int buildingPosition = new Vector3Int(x, 0, 0) * GlobalProperties.WorldTileSize;
            
            if (!IsBuildingInAreaRange(_currentBuildingArea, buildingPosition)) 
                while (!IsBuildingInAreaRange(_currentBuildingArea, buildingPosition))
                    if (buildingPosition.x < 0) buildingPosition.x += 1 * GlobalProperties.WorldTileSize;
                    else if (buildingPosition.x > 0) buildingPosition.x -= 1 * GlobalProperties.WorldTileSize;

            buildingPosition /= GlobalProperties.WorldTileSize;
            
            _currentBuilding = Instantiate(
                _currentBuildingData.Prefab.gameObject, 
                _currentBuildingArea.GridMap.GetWorldPosition(buildingPosition.x, buildingPosition.y, _currentBuildingArea.transform.position), 
                Quaternion.identity, 
                _currentBuildingArea.transform);
            
            _currentPlacingPosition = Vector3Int.FloorToInt(buildingPosition) * GlobalProperties.WorldTileSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void MoveCurrentBuilding(Vector3Int direction)
        {
            direction *= GlobalProperties.WorldTileSize;
            int newBuildPosX = _currentPlacingPosition.x + direction.x;
            
            if (Mathf.Abs(_currOffset.x + direction.x) > _maxXOffset) return;
            if (!_currentBuildingArea.GridMap.IsTileInRange(newBuildPosX, _currentPlacingPosition.y)) return;
            if (!_currentBuildingArea.GridMap.IsTileInRange(newBuildPosX, _currentPlacingPosition.y, _currentBuildingData.Size.x)) return;
            
            _currOffset += direction;
            _currentPlacingPosition += direction;
            
            _currentBuilding.transform.localPosition = CheckBuildingCurrentPosition();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
                _currentBuildingArea.GridMap.GetTileWithNeighbours(
                    Vector2Int.FloorToInt(_currentBuilding.transform.localPosition) / GlobalProperties.WorldTileSize, 
                    _currentBuildingData.Size);
            
            if (!_currentBuildingArea.CanPlaceBuilding(buildingArea, _currentBuildingData.RequiredCellContent)) {
                Debug.LogWarning("Can't build this object there");
                return;
            }
            
            _currentBuilding.GetComponent<Construction>()
                .InitializeConstruction(_currentBuildingData, new Material(buildingFadeMaterial));
            _currentBuildingArea.AddBuilding(_currentBuilding.GetComponent<Building>(), _currentBuildingData);

            switch (_currentBuildingData.RequiredCellContent) {
                case CellContentType.Null:
                    break;
                
                case CellContentType.Nothing:
                    break;
                
                case CellContentType.WoodResource:
                    break;
                
                case CellContentType.StoneResource:
                    if (_currentBuildingArea.GridMap.GetCellAt(buildingArea[0].x, buildingArea[0].y).resourceToGatherData
                        is Stone stone)
                        stone.Workplace = _currentBuilding.GetComponent<Workplaces.Workplace>();
                    break;
                
                case CellContentType.Building:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentBuilding = null;
            _currentBuildingData = null;
            _currentBuildingArea = null;
            _currentPlacingPosition = Vector3Int.zero;
            _currOffset = Vector3Int.zero;
            
            
            Managers.I.Input.SetState(InputManager.PlayerActions);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelBuilding()
        {
            DestroyImmediate(_currentBuilding);
            
            _currentBuilding = null;
            _currentBuildingData = null;
            _currOffset = Vector3Int.zero;
        }
    }
}
