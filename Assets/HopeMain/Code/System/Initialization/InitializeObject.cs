using UnityEngine;

namespace Code.System.Initialization
{
    public enum InitializeObjectMode
    {
        NEW, LOAD
    }
    
    public abstract class InitializeObject : MonoBehaviour
    {
        [SerializeField] private InitializeObjectMode initializeMode;
        
        public abstract void InitializeMe();
    }
}