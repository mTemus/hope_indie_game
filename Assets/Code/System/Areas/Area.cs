using System.Collections.Generic;
using System.Linq;
using Code.Map.Building;
using Code.Map.Resources.ResourceToGather;
using Code.System.Grid;
using Code.System.Properties;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.System.Areas
{
    public enum AreaType
    {
        VILLAGE,
        FOREST,
        EMPTY,
    }

    public abstract class Area : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private AreaType type;
        [SerializeField] private float width = 100f;
        [SerializeField] private float height = 30f;

        private readonly List<Villager> villagers = new List<Villager>();
        private readonly List<Building> buildings = new List<Building>();
        private readonly List<ResourceToGather> resourcesToGather = new List<ResourceToGather>();
        
        private GameObject playerObject;
        private GridMap gridMap;
        
        public GridMap GridMap => gridMap;
        public AreaType Type => type;
        public float Width => width;
        public float Height => height;
        
        void Awake()
        {
            int widthTileCnt = (int) (width / GlobalProperties.WorldTileSize);
            int heightTileCnt = (int) (height / GlobalProperties.WorldTileSize);
        
            gridMap = new GridMap(widthTileCnt, heightTileCnt, GlobalProperties.WorldTileSize);
        }

        public bool IsPlayerInArea() 
        {
            return playerObject != null;
        }

        public Vector3 GetAreaWorldPosition 
            => transform.position;
        
        public void SetPlayerToArea(GameObject player)
        {
            playerObject = player;
            playerObject.transform.SetParent(transform);
        }

        public void ClearPlayerInArea()
        {
            playerObject.transform.SetParent(transform.root);
            playerObject = null;
        }
        
        public void AddBuilding(Building building, BuildingData buildingData)
        {
            Transform buildingTransform = building.transform;
            buildingTransform.SetParent(transform);
            
            Vector3Int currBuildPos = Vector3Int.FloorToInt(buildingTransform.localPosition);
            
            List<Vector2Int> buildingArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), 
                    new Vector2Int(buildingData.Size.x, buildingData.Size.y));
            
            gridMap.SetBuildingValueAtArea(buildingArea, building);
            buildings.Add(building);
            Debug.Log("Add building " + building.name + "  to area "+ gameObject.name + ".");
        }
        
         public void AddResourceToGather(ResourceToGather resourceToGather)
        {
            Transform resourceTransform = resourceToGather.transform;
            resourceTransform.SetParent(transform);

            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceTransform.localPosition);

            List<Vector2Int> resourceArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            gridMap.SetResourceToGatherValueAtArea(resourceArea, resourceToGather);
            resourcesToGather.Add(resourceToGather);
            Debug.Log("Add resource " + resourceToGather.name + "  to area "+ gameObject.name + ".");
        }

        public void RemoveResourceToGather(ResourceToGather resourceToGather)
        {
            Vector3Int currResourcePos = Vector3Int.FloorToInt(resourceToGather.transform.localPosition);
            List<Vector2Int> resourceArea = gridMap.GetTileWithNeighbours(
                new Vector2Int(currResourcePos.x / GlobalProperties.WorldTileSize, currResourcePos.y),
                new Vector2Int(resourceToGather.Size.x, resourceToGather.Size.y));

            gridMap.SetResourceToGatherValueAtArea(resourceArea, null);
            resourcesToGather.Remove(resourceToGather);
            Debug.LogError("Remove resource " + resourceToGather.name + "  to area "+ gameObject.name + ".");
        }
        
        public bool CanPlaceBuilding(List<Vector2Int> buildingArea) =>
            buildingArea
                .Select(tilePos => gridMap.GetCellAt(tilePos.x, tilePos.y))
                .All(cell => cell.CanBuild());
        
        public void AddVillager(Villager villager)
        {
            villager.transform.SetParent(transform);
            villagers.Add(villager);
            
            Debug.Log("Add building " + villager.name + "  to area "+ gameObject.name + ".");
        }
    }
}