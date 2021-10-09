using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.World.Resources.ResourceToGather
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "Resource to gather data", menuName = "Game Data/Map Objects/Resource to Gather Data", order = 0)]
    public class Data : ScriptableObject
    {
        [Header("Resource properties")]
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount;
        [SerializeField] private int maximumGatherers;

        [Header("World data")] 
        [SerializeField] private AreaType[] occurAreas;

        public AreaType[] OccurAreas => occurAreas;
        public ResourceType ResourceType => resourceType;
        public int Amount => amount;
        public int MaximumGatherers => maximumGatherers;
    }
}
