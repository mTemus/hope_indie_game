using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.System.Initialization
{
    public class InitializeResourceToGather : InitializeObject
    {
        [SerializeField] private Data resourceToGatherData;
        
        public override void InitializeMe()
        {
            ResourceToGatherBase rtg = GetComponent<ResourceToGatherBase>();
            rtg.Initialize(resourceToGatherData);
            
            Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .AddResourceToGather(rtg);
            
            DestroyImmediate(this);
        }
    }
}
