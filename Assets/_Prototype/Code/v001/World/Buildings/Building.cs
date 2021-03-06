using _Prototype.Code.v001.World.Buildings.Modules;
using UnityEngine;

namespace _Prototype.Code.v001.World.Buildings
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Building : MonoBehaviour
    {
        [Header("Building Data")]
        [SerializeField] private Data data;
        
        [Header("Building Modules")]
        [SerializeField] private BuildingStorage storage;

        public Vector3 PivotedPosition =>
            transform.position + data.EntrancePivot;
        public Vector3 PivotedLocalPosition =>
            transform.localPosition + data.EntrancePivot;

        public Data Data => data;
        public BuildingStorage Storage => storage;
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void Initialize();
    }
}
