using UnityEngine;

namespace Code.Villagers.Brain.Layers
{
    public abstract class BrainLayer : MonoBehaviour
    {
        protected Villager_Brain brain;

        public virtual void Initialize(Villager_Brain villagerBrain)
        {
            brain = villagerBrain;
        }
    }
}
