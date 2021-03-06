using Code.Map.Building.Systems;
using UnityEngine;

namespace Code.System
{
    public class Systems : MonoBehaviour
    {
        [SerializeField] private BuildingSystem building;
        
        private static Systems _instance;
        
        private void Awake()
        {
            _instance = this;
        }


        public static Systems Instance => _instance;

        public BuildingSystem Building => building;
    }
}
