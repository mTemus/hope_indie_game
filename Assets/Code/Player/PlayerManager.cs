using Code.System;
using Code.System.Areas;
using UnityEngine;

namespace Code.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        
        private void Start()
        {
            Area area = Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(player.transform.position));
            Debug.LogWarning("Start area: " + area.name);
            area.SetPlayerToArea(player.gameObject);
        }

        public Vector2 GetPlayerPosition() 
            => player.transform.position;

        public Vector2 GetPlayerLocalPosition()
            => player.transform.localPosition;

        public PlayerController Player => player;

        public GameObject PlayerGO => player.gameObject;
    }
}
