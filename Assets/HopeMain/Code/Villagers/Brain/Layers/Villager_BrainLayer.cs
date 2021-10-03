using UnityEngine;

namespace Code.Villagers.Brain.Layers
{
    public abstract class Villager_BrainLayer : MonoBehaviour
    {
        public abstract void Initialize(Villager_Brain villagerBrain);
    }
}
