using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public abstract class BrainLayer : MonoBehaviour
    {
        public abstract void Initialize(Brain brain);
    }
}
