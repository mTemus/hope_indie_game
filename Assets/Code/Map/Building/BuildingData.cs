using UnityEngine;

namespace Code.Map.Building
{
    [CreateAssetMenu(fileName = "Building Data", menuName = "Map Objects", order = 0)]
    public class BuildingData : ScriptableObject
    {
        public Transform prefab;
        public Transform visual;
        
        public string buildingName;
        public int width;
        public int height;
        public float xPivot;
        public float yPivot;
        
        
        
    }
}