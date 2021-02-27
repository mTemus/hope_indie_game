using UnityEngine;

namespace Code.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private static PlayerManager _instance;
    
        private void Awake()
        {
            _instance = this;
        }

        public Vector2 GetPlayerPosition() 
            => player.transform.position;
    
        public GameObject Player => player;

        public static PlayerManager Instance => _instance;
    }
}
