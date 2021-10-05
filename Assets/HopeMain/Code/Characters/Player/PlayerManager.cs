using HopeMain.Code.AI.Player.Brain;
using HopeMain.Code.System;
using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.Characters.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Brain player;
        
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

        public Brain Player => player;

        public GameObject PlayerGO => player.gameObject;
    }
}
