using System.Collections.Generic;
using Code.Villagers.Entity;
using UnityEngine;

namespace Code.Map.Resources.ResourceToGather
{
    public enum ResourceToGatherType
    {
        Single, 
        Spot
    }
    
    public abstract class ResourceToGather : MonoBehaviour
    {
        protected Resource resource;
        protected ResourceToGatherType type;
        protected int maximumGatherers;

        private readonly List<Villager> gatherers = new List<Villager>();
        
        public abstract void OnGatherStart(Villager worker);
        public abstract void OnGatherEnd(Villager worker);
        public abstract void Gather(Villager worker);

        public void Initialize(ResourceToGatherData resourceToGatherData)
        {
            resource = new Resource(resourceToGatherData.ResourceType, resourceToGatherData.Amount);
            type = resourceToGatherData.ResourceToGatherType;
            maximumGatherers = resourceToGatherData.MaximumGatherers;
        }
        
        public void RegisterGatherer(Villager worker)
        {
            gatherers.Add(worker);
        }

        public void UnregisterGatherer(Villager worker)
        {
            gatherers.Remove(worker);
        }
        
        public bool CanGather() =>
            gatherers.Count < maximumGatherers;
    }
}
