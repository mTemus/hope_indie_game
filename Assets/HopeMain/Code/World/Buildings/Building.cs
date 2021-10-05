using HopeMain.Code.World.Buildings.Modules;
using UnityEngine;

namespace HopeMain.Code.World.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        [Header("Building Data")]
        [SerializeField] private BuildingData data;
        
        [Header("Building Modules")]
        [SerializeField] private BuildingStorageModule storage;

        public Vector3 PivotedPosition =>
            transform.position + data.EntrancePivot;
        public Vector3 PivotedLocalPosition =>
            transform.localPosition + data.EntrancePivot;

        public BuildingData Data => data;
        public BuildingStorageModule Storage => storage;
        
        public abstract void Initialize();
    }
}
