using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public abstract class BrainLayer : MonoBehaviour
    {
        protected Villager_Brain brain;

        public abstract void Initialize(Villager_Brain villagerBrain);
    }
}