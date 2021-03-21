using UnityEngine;

namespace Code.Map.Building
{
    public class Building : MonoBehaviour
    {
        private Vector3 entrancePivot;
        private Vector2Int buildingSize;
        
        public void SetEntrancePivot(float x, float y, float z)
        {
            entrancePivot = new Vector3(x, y, z);
        }

        public void SetBuildingSize(float x, float y)
        {
            buildingSize = new Vector2Int((int) x, (int) y);
        }
        
        public void SetEntrancePivot(Vector3 newPivot)
        {
            entrancePivot = newPivot;
        }
        
        public Vector3 EntrancePivot => entrancePivot;

        public Vector2Int BuildingSize => buildingSize;
    }
}
