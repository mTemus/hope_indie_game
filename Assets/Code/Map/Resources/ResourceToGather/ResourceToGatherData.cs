using Code.System.Areas;
using UnityEngine;

namespace Code.Map.Resources.ResourceToGather
{
    [CreateAssetMenu(fileName = "Resource to gather data", menuName = "Game Data/Map Objects/Resource to Gather Data", order = 0)]
    public class ResourceToGatherData : ScriptableObject
    {
        [Header("Resource properties")]
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private ResourceToGatherType resourceToGatherType;
        [SerializeField] private int amount;
        [SerializeField] private int maximumGatherers;

        [Header("World data")] 
        [SerializeField] private AreaType[] occurAreas;

        public AreaType[] OccurAreas => occurAreas;
        public ResourceType ResourceType => resourceType;
        public int Amount => amount;
        public int MaximumGatherers => maximumGatherers;
        public ResourceToGatherType ResourceToGatherType => resourceToGatherType;
    }
}
