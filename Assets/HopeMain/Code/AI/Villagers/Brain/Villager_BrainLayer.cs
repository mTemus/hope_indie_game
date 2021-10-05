using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public abstract class Villager_BrainLayer : MonoBehaviour
    {
        public abstract void Initialize(Villager_Brain villagerBrain);
    }
}
