using HopeMain.Code.World.Buildings.Systems;
using UnityEngine;

namespace HopeMain.Code.System
{
    public class Systems : MonoBehaviour
    {
        [SerializeField] private BuildingSystem building;
        
        private static Systems _instance;
        
        private void Awake()
        {
            _instance = this;
        }
        
        public BuildingSystem Building => building;
        
        public static Systems I => _instance;
    }
}
