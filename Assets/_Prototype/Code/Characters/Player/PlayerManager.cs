using _Prototype.Code.AI.Player.Brain;
using _Prototype.Code.System;
using _Prototype.Code.World.Areas;
using UnityEngine;

namespace _Prototype.Code.Characters.Player
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Brain player;
        
        private void Start()
        {
            Area area = Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(player.transform.position));
            Debug.LogWarning("Start area: " + area.name);
            area.SetPlayerToArea(player.gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerPosition() 
            => player.transform.position;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerLocalPosition()
            => player.transform.localPosition;

        public Brain Player => player;

        public GameObject PlayerGO => player.gameObject;
    }
}
