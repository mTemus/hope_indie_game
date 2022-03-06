using UnityEngine;

namespace _Prototype.Code.System.Initialization
{
    public enum InitializeObjectMode
    {
        New, Load
    }
    
    public abstract class InitializeObject : MonoBehaviour
    {
        [SerializeField] private InitializeObjectMode initializeMode;
        
        public abstract void InitializeMe();
    }
}