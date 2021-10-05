using System;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public enum ResourceGatheringFlag
    {
        GO_TO_WORKPLACE,
        FIND_CLOSEST_RESOURCE,
        GO_TO_RESOURCE,
        GATHER_RESOURCE,
        DELIVER_RESOURCE_TO_WORKPLACE
    }
    
    public abstract class ResourceGathering : Task
    {
        protected ResourceType resourceType;
        protected AreaType[] gatherAreas;
        
        protected ResourceGatheringFlag currentGatheringState;
        protected ResourceToGatherBase resourceToGather;
        protected Vector3 resourcePosition;
        
        protected int gatheringSocketId;

        public Action<Resource> onResourceDelivery;

        public abstract void DepleteCurrentResource();
    }
}
