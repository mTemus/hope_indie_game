using UnityEngine;

namespace _Prototype.Code.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BrainLayer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brain"></param>
        public abstract void Initialize(Brain brain);
    }
}
