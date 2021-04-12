using Code.Map.Building.Buildings.Modules;
using UnityEngine;

namespace Code.Map.Building
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] private BuildingStorageModule storage;
        
        private BuildingData data;
        
        public Vector3 PivotedPosition =>
            transform.position + data.EntrancePivot;

        public Vector3 PivotedLocalPosition =>
            transform.localPosition + data.EntrancePivot;

        public BuildingData Data
        {
            get => data;
            set => data = value;
        }

        public BuildingStorageModule Storage => storage;
    }
}
