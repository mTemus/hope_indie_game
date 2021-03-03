using Code.System;
using Code.System.Area;
using UnityEngine;

namespace Code.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        
        private void Start()
        {
            Area area = Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(player.transform.position));
            Debug.LogWarning("Start area: " + area.name);
            area.SetPlayerToArea(player);
        }

        public Vector2 GetPlayerPosition() 
            => player.transform.position;
    
        public GameObject Player => player;
    }
}
