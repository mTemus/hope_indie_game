using UnityEngine;

namespace Code.Map.Building
{
    public class Building : MonoBehaviour
    {
        private Vector3 entrancePivot;
        
        public void SetEntrancePivot(float x, float y, float z)
        {
            entrancePivot = new Vector3(x, y, z);
        }
        
        public void SetEntrancePivot(Vector3 newPivot)
        {
            entrancePivot = newPivot;
        }
        
        public Vector3 EntrancePivot => entrancePivot;
    }
}
