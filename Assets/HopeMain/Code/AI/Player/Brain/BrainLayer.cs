using UnityEngine;

namespace HopeMain.Code.AI.Player.Brain
{
    public abstract class BrainLayer : MonoBehaviour
    {
        public abstract void Initialize(Brain brain);
    }
}
