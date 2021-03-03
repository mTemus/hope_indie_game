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

        private GameObject playerObject;
        private GridMap gridMap;
        
        void Awake()
        {
            int widthTileCnt = (int) (width / GlobalProperties.TileSize);
            int heightTileCnt = (int) (height / GlobalProperties.TileSize);
        
            gridMap = new GridMap(widthTileCnt, heightTileCnt, GlobalProperties.TileSize);
        }

        public bool IsPlayerInArea() 
        {
            return playerObject != null;
        }

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

        public GridMap GridMap => gridMap;

        public AreaType Type => type;

        public float Width => width;

        public float Height => height;
    }
}