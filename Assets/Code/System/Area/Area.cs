using System.Collections.Generic;
using Code.Map.Building;
using Code.System.Grid;
using Code.System.Properties;
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

        private List<Building> buildings = new List<Building>();
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
            buildings.Add(building);
            
            Debug.Log("Add building " + building.name + "  to area "+ gameObject.name + ".");
        }

        public GridMap GridMap => gridMap;

        public AreaType Type => type;

        public float Width => width;

        public float Height => height;
    }
}