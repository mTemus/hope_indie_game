using _Prototype.Code.v001.World.Buildings.Systems;
using UnityEngine;

namespace _Prototype.Code.v001.System
{
    /// <summary>
    /// 
    /// </summary>
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
