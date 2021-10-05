using HopeMain.Code.World.Buildings.Modules;
using UnityEngine;

namespace HopeMain.Code.World.Buildings
{
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
        
        public abstract void Initialize();
    }
}
