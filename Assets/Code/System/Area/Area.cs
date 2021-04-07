using System.Collections.Generic;
using System.Linq;
using Code.Map.Building;
using Code.Map.Building.Buildings.Components.Resources;
using Code.System.Grid;
using Code.System.Properties;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.System.Area
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
        private GameObject playerObject;
        private GridMap gridMap;
        
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

        public void AddBuilding(Building building)
        {
            building.transform.SetParent(transform);
            buildings.Add(building);
        }
        
        
        public void AddBuilding(Building building, BuildingData buildingData)
        {
            Transform buildingTransform = building.transform;
            buildingTransform.SetParent(transform);
            
            Vector3Int currBuildPos = Vector3Int.FloorToInt(buildingTransform.localPosition);
            
            List<Vector2Int> buildingArea =
                gridMap.GetTileWithNeighbours(new Vector2Int(currBuildPos.x / GlobalProperties.WorldTileSize, currBuildPos.y), new Vector2Int(buildingData.Size.x, buildingData.Size.y));
            
            foreach (Vector2Int tilePos in buildingArea) {
                Cell cell = gridMap.GetCellAt(tilePos.x, tilePos.y);
                cell.SetBuildingAtCell(building);
            }
            
            buildings.Add(building);
            Debug.Log("Add building " + building.name + "  to area "+ gameObject.name + ".");
        }

        public bool CanPlaceBuilding(List<Vector2Int> buildingArea) =>
            buildingArea
                .Select(tilePos => gridMap.GetCellAt(tilePos.x, tilePos.y))
                .All(cell => cell.CanBuild());
        
        public void FillTiles(List<Vector2Int> buildingArea, Transform building)
        {
            foreach (Vector2Int tilePos in buildingArea) 
                gridMap.GetCellAt(tilePos.x, tilePos.y).SetBuildingAtCell(building.GetComponent<Building>());
        }

        public void AddVillager(Villager villager)
        {
            villager.transform.SetParent(transform);
            villagers.Add(villager);
            
            Debug.Log("Add building " + villager.name + "  to area "+ gameObject.name + ".");
        }

        public Warehouse GetWarehouse()
        {
            foreach (Building building in buildings) 
                if (building.TryGetComponent(out Warehouse w)) 
                    return w;
            
            return null;
        }

        public GridMap GridMap => gridMap;

        public AreaType Type => type;

        public float Width => width;

        public float Height => height;
    }
}