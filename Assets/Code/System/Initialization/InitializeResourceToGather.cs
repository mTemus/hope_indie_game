using Code.Map.Resources.ResourceToGather;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeResourceToGather : InitializeObject
    {
        [SerializeField] private ResourceToGatherData resourceToGatherData;
        
        public override void InitializeMe()
        {
            ResourceToGather rtg = GetComponent<ResourceToGather>();
            rtg.Initialize(resourceToGatherData);
            
            Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .AddResourceToGather(rtg);
        }
    }
}
