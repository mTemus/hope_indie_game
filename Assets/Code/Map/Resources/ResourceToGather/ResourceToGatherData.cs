using UnityEngine;

namespace Code.Map.Resources.ResourceToGather
{
    [CreateAssetMenu(fileName = "Resource to gather data", menuName = "Game Data/Map Objects/Resource to Gather Data", order = 0)]
    public class ResourceToGatherData : ScriptableObject
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private ResourceToGatherType resourceToGatherType;
        [SerializeField] private int amount;
        [SerializeField] private int maximumGatherers;

        public ResourceType ResourceType => resourceType;
        public int Amount => amount;
        public int MaximumGatherers => maximumGatherers;
        public ResourceToGatherType ResourceToGatherType => resourceToGatherType;
    }
}
