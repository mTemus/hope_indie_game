using System;
using System.Collections.Generic;
using System.Linq;
using Code.AI;
using Code.Environment.Parallax;
using Code.Map.Building;
using Code.Map.Resources;
using Code.Map.Resources.ResourceToGather;
using Code.Map.Resources.ResourceToGather.ResourcesToGather;
using Code.System.Assets;
using Code.System.Grid;
using Code.System.Properties;
using Code.Villagers.Brain;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.System.Areas
{
    public enum AreaType
    {
        VILLAGE,
        FOREST,
        HIGHLANDS,
        EMPTY,
    }

    public abstract class Area : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private AreaType type;
        [SerializeField] private float width = 100f;
        [SerializeField] private float height = 30f;

        [Header("Environment")] 
        [SerializeField] private ParallaxControllerLocal parallax;

        private readonly List<Villager> villagers = new List<Villager>();
        private readonly List<Building> buildings = new List<Building>();
        private readonly List<ResourceToGather> resourcesToGather = new List<ResourceToGather>();
        
        private GameObject playerObject;
        private GridMap gridMap;
        
        public GridMap GridMap => gridMap;
        public AreaType Type => type;
        public float Width => width;
        public float Height => height;
        public Vector3 AreaWorldPosition => transform.position;
        public bool IsPlayerInArea => playerObject != null;

        void Awake()
        {
            int widthTileCnt = (int) (width / GlobalProperties.WorldTileSize);
            int heightTileCnt = (int) (height / GlobalProperties.WorldTileSize);
        
            gridMap = new GridMap(widthTileCnt, heightTileCnt, GlobalProperties.WorldTileSize);
        }

        #region Area

        public void SetVisitorWalkingAudio(GameObject visitor)
        {
            visitor
                .GetComponent<EntityBrain>().onWalkingSoundSet
                .Invoke(AssetsStorage.I
                    .GetAudioClipByName(AssetSoundType.Walking, name.ToLower()));
        }

        public bool ContainsEntity(GameObject visitor)
        {
            return visitor.tag switch {
                "Player" => playerObject != null,
                "Villager" => villagers.Contains(visitor.GetComponent<Villager>()),
                _ => throw new Exception("AREA --- CAN'T FIND ENTITY WITH TAG: " + gameObject.tag)
            };
        }

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

        public float ClampInArea(float entityX, float entityWidth)
        {
            float leftCorner = transform.localPosition.x;
            return Mathf.Clamp(entityX, leftCorner, leftCorner + width - entityWidth);
        }
        
        #endregion

        #region Player
        
        private void PlayerEnterArea(GameObject player)
        {
            playerObject = player;
            playerObject.GetComponent<EntityBrain>().CurrentArea = this;
            playerObject.transform.SetParent(transform);
            parallax.Move(Vector3.zero);
            SetCurrentLocalParallax();
            Debug.LogWarning("Player is in " + name);
        }

        private void PlayerExitArea(GameObject player)
        {
            parallax.Move(Vector3.zero);
            playerObject.transform.SetParent(transform.root);
            playerObject.GetComponent<EntityBrain>().CurrentArea = null;
            playerObject = null;
        }
        
        public void SetPlayerToArea(GameObject player)
        {
            playerObject = player;
            player.transform.SetParent(transform);
        }
        
        #endregion

        #region Buildings

        public void AddBuilding(Building building, BuildingData buildingData)
        {
            Transform buildingTransform = building.transform;
            buildingTransform.SetParent(transform);
            
            Vector3Int currBuildPos = Vector3Int.FloorToInt(buildingTransform.localPosition);
            
            List<Vector2Int> buildingArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), 
                new Vector2Int(buildingData.Size.x, buildingData.Size.y));
            
            gridMap.SetBuildingInGrid(buildingArea, building);
            buildings.Add(building);
            Debug.Log("Add building " + building.name + "  to area "+ gameObject.name + ".");
        }
        
        public bool CanPlaceBuilding(List<Vector2Int> buildingArea, CellContentType requiredCellContent)
        {
            if (!buildingArea
                .FindAll(tilePos => tilePos.y == buildingArea[0].y)
                .Select(baseTilePos => gridMap.GetCellAt(baseTilePos.x, baseTilePos.y))
                .All(cell => cell.content == requiredCellContent)) 
                return false;
            
            if (!buildingArea
                .FindAll(tilePos => tilePos.y != buildingArea[0].y)
                .Select(baseTilePos => gridMap.GetCellAt(baseTilePos.x, baseTilePos.y))
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
                    if (gridMap.GetCellAt(buildingArea[0].x, buildingArea[0].y).resourceToGatherData is StoneToGather
                        {
                            hasWorkplace: true
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

        public void AddResourceToGather(ResourceToGather resourceToGather)
        {
            Transform resourceTransform = resourceToGather.transform;
            resourceTransform.SetParent(transform);

            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceTransform.localPosition);

            List<Vector2Int> resourceArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            gridMap.SetResourceToGatherInGrid(resourceArea, resourceToGather);
            resourcesToGather.Add(resourceToGather);
        }

        public void RemoveResourceToGather(ResourceToGather resourceToGather)
        {
            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceToGather.transform.localPosition);
            List<Vector2Int> resourceArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            gridMap.SetResourceToGatherInGrid(resourceArea, null);
            resourcesToGather.Remove(resourceToGather);
            Debug.LogError("Remove resource " + resourceToGather.name + "  to area "+ gameObject.name + ".");
        }
        
        public ResourceToGather GetClosestResourceToGatherByType(Vector3 position, ResourceType resourceType)
        {
            List<ResourceToGather> resources = resourcesToGather
                .Where(resourceToGather => resourceToGather.Resource.Type == resourceType)
                .ToList();

            ResourceToGather closestResource = resources[0];
            float bestDistance = Vector3.Distance(position, closestResource.transform.position);

            foreach (ResourceToGather resource in resources) {
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
            villager.GetComponent<Villager_Brain>().CurrentArea = this;
            villager.transform.SetParent(transform);
        }

        private void VillagerExitArea(GameObject villager)
        {
            villagers.Remove(villager.GetComponent<Villager>());
            villager.transform.SetParent(transform.parent);
        }
        
        public void AddVillager(Villager villager)
        {
            villager.Brain.CurrentArea = this;
            villager.transform.SetParent(transform);
            villagers.Add(villager);
            
            Debug.Log("Add villager " + villager.name + "  to area "+ gameObject.name + ".");
        }

        #endregion

        #region Environment

        private void SetCurrentLocalParallax()
        {
            //TODO: remove this null, it is only for now
            if (parallax == null) {
                Managers.I.Environment.SetLocalParallax(null);
                return;
            }
            Managers.I.Environment.SetLocalParallax(parallax);
        }

        #endregion
    }
}