using System;
using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.AI;
using _Prototype.Code.v001.AI.Villagers.Brain;
using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.Environment.Parallax;
using _Prototype.Code.v001.GameProperties;
using _Prototype.Code.v001.System;
using _Prototype.Code.v001.System.Assets;
using _Prototype.Code.v001.World.Buildings;
using _Prototype.Code.v001.World.Grid;
using _Prototype.Code.v001.World.Grid.Cell;
using _Prototype.Code.v001.World.Resources;
using _Prototype.Code.v001.World.Resources.ResourceToGather;
using UnityEngine;
using Data = _Prototype.Code.v001.World.Buildings.Data;

namespace _Prototype.Code.v001.World.Areas
{
    /// <summary>
    /// 
    /// </summary>
    public enum AreaType
    {
        Village,
        Forest,
        Highlands,
        Empty,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Area : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private AreaType type;
        [SerializeField] private float width = 100f;
        [SerializeField] private float height = 30f;

        [Header("Environment")] 
        [SerializeField] private LocalParallaxController localParallax;

        private readonly List<Villager> _villagers = new List<Villager>();
        private readonly List<Building> _buildings = new List<Building>();
        private readonly List<ResourceToGatherBase> _resourcesToGather = new List<ResourceToGatherBase>();
        
        private GameObject _playerObject;
        private GridMap _gridMap;
        
        public GridMap GridMap => _gridMap;
        public AreaType Type => type;
        public float Width => width;
        public float Height => height;
        public Vector3 AreaWorldPosition => transform.position;
        public bool IsPlayerInArea => _playerObject != null;

        private void Awake()
        {
            int widthTileCnt = (int) (width / GlobalProperties.WorldTileSize);
            int heightTileCnt = (int) (height / GlobalProperties.WorldTileSize);
        
            _gridMap = new GridMap(widthTileCnt, heightTileCnt, GlobalProperties.WorldTileSize);
        }

        #region Area

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public void SetVisitorWalkingAudio(GameObject visitor)
        {
            visitor
                .GetComponent<EntityBrain>().walkingSoundSet
                .Invoke(AssetsStorage.I
                    .GetAudioClipByName(SoundType.Walking, name.ToLower()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ContainsEntity(GameObject visitor)
        {
            return visitor.tag switch {
                "Player" => _playerObject != null,
                "Villager" => _villagers.Contains(visitor.GetComponent<Villager>()),
                _ => throw new Exception("AREA --- CAN'T FIND ENTITY WITH TAG: " + gameObject.tag)
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        /// <exception cref="Exception"></exception>
        public void HandleEnteringEntity(GameObject visitor)
        {
            switch (visitor.tag) {
                case "Player":
                    PlayerEnterArea(visitor);
                    break;
                
                case "Villager":
                    VillagerEnterArea(visitor);
                    break;
                
                default:
                    throw new Exception("AREA --- CAN'T HANDLE VISITOR WITH TAG: " + gameObject.tag);
            }
            
            SetVisitorWalkingAudio(visitor);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaver"></param>
        /// <exception cref="Exception"></exception>
        public void HandleLeavingEntity(GameObject leaver)
        {
            switch (leaver.tag) {
                case "Player":
                    PlayerExitArea(leaver);
                    break;
                
                case "Villager":
                    VillagerExitArea(leaver);
                    break;
                
                default:
                    throw new Exception("AREA --- CAN'T HANDLE VISITOR WITH TAG: " + gameObject.tag);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityX"></param>
        /// <param name="entityWidth"></param>
        /// <returns></returns>
        public float ClampInArea(float entityX, float entityWidth)
        {
            float leftCorner = transform.localPosition.x;
            return Mathf.Clamp(entityX, leftCorner, leftCorner + width - entityWidth);
        }
        
        #endregion

        #region Player
        
        private void PlayerEnterArea(GameObject player)
        {
            _playerObject = player;
            _playerObject.GetComponent<EntityBrain>().CurrentArea = this;
            _playerObject.transform.SetParent(transform);
            localParallax.Move(Vector3.zero);
            SetCurrentLocalParallax();
            Debug.LogWarning("Player is in " + name);
        }

        private void PlayerExitArea(GameObject player)
        {
            localParallax.Move(Vector3.zero);
            _playerObject.transform.SetParent(transform.root);
            _playerObject.GetComponent<EntityBrain>().CurrentArea = null;
            _playerObject = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void SetPlayerToArea(GameObject player)
        {
            _playerObject = player;
            player.transform.SetParent(transform);
        }
        
        #endregion

        #region Buildings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="building"></param>
        /// <param name="buildingData"></param>
        public void AddBuilding(Building building, Data buildingData)
        {
            Transform buildingTransform = building.transform;
            buildingTransform.SetParent(transform);
            
            Vector3Int currBuildPos = Vector3Int.FloorToInt(buildingTransform.localPosition);
            
            List<Vector2Int> buildingArea = _gridMap.GetTileWithNeighbours(
                new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), 
                new Vector2Int(buildingData.Size.x, buildingData.Size.y));
            
            _gridMap.SetBuildingInGrid(buildingArea, building);
            _buildings.Add(building);
            Debug.Log("Add building " + building.name + "  to area "+ gameObject.name + ".");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingArea"></param>
        /// <param name="requiredCellContent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool CanPlaceBuilding(List<Vector2Int> buildingArea, CellContentType requiredCellContent)
        {
            if (!buildingArea
                .FindAll(tilePos => tilePos.y == buildingArea[0].y)
                .Select(baseTilePos => _gridMap.GetCellAt(baseTilePos.x, baseTilePos.y))
                .All(cell => cell.content == requiredCellContent)) 
                return false;
            
            if (!buildingArea
                .FindAll(tilePos => tilePos.y != buildingArea[0].y)
                .Select(baseTilePos => _gridMap.GetCellAt(baseTilePos.x, baseTilePos.y))
                .All(cell => cell.content == requiredCellContent || cell.content == CellContentType.Nothing)) 
                return false;

            switch (requiredCellContent) {
                case CellContentType.Null:
                    break;
                
                case CellContentType.Nothing:
                    break;
                
                case CellContentType.WoodResource:
                    break;
                
                case CellContentType.StoneResource:
                    if (_gridMap.GetCellAt(buildingArea[0].x, buildingArea[0].y).resourceToGatherData is Stone
                        {
                            HasWorkplace: true
                        })
                        return false;
                    break;
                
                case CellContentType.Building:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(requiredCellContent), requiredCellContent, null);
            }

            return true;
        }

        #endregion

        #region Resources to Gather

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceToGather"></param>
        public void AddResourceToGather(ResourceToGatherBase resourceToGather)
        {
            Transform resourceTransform = resourceToGather.transform;
            resourceTransform.SetParent(transform);

            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceTransform.localPosition);

            List<Vector2Int> resourceArea = _gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            _gridMap.SetResourceToGatherInGrid(resourceArea, resourceToGather);
            _resourcesToGather.Add(resourceToGather);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceToGather"></param>
        public void RemoveResourceToGather(ResourceToGatherBase resourceToGather)
        {
            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceToGather.transform.localPosition);
            List<Vector2Int> resourceArea = _gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            _gridMap.SetResourceToGatherInGrid(resourceArea, null);
            _resourcesToGather.Remove(resourceToGather);
            Debug.LogError("Remove resource " + resourceToGather.name + "  to area "+ gameObject.name + ".");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public ResourceToGatherBase GetClosestResourceToGatherByType(Vector3 position, ResourceType resourceType)
        {
            List<ResourceToGatherBase> resources = _resourcesToGather
                .Where(resourceToGather => resourceToGather.Resource.Type == resourceType)
                .ToList();

            ResourceToGatherBase closestResource = resources[0];
            float bestDistance = Vector3.Distance(position, closestResource.transform.position);

            foreach (ResourceToGatherBase resource in resources) {
                float distance = Vector3.Distance(position, resource.transform.position);

                if (bestDistance < distance) continue;
                if (!resource.CanGather) continue;
                bestDistance = distance;
                closestResource = resource;
            }

            return closestResource;
        }

        #endregion

        #region Villagers

        private void VillagerEnterArea(GameObject villager)
        {
            villager.GetComponent<Brain>().CurrentArea = this;
            villager.transform.SetParent(transform);
        }

        private void VillagerExitArea(GameObject villager)
        {
            _villagers.Remove(villager.GetComponent<Villager>());
            villager.transform.SetParent(transform.parent);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="villager"></param>
        public void AddVillager(Villager villager)
        {
            villager.Brain.CurrentArea = this;
            villager.transform.SetParent(transform);
            _villagers.Add(villager);
            
            Debug.Log("Add villager " + villager.name + "  to area "+ gameObject.name + ".");
        }

        #endregion

        #region Environment

        private void SetCurrentLocalParallax()
        {
            //TODO: remove this null, it is only for now
            if (localParallax == null) {
                Managers.I.Environment.SetLocalParallax(null);
                return;
            }
            Managers.I.Environment.SetLocalParallax(localParallax);
        }

        #endregion
    }
}