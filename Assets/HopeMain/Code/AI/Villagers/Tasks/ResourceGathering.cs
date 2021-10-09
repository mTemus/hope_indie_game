using System;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResourceGatheringFlag
    {
        GOToWorkplace,
        FindClosestResource,
        GOToResource,
        GatherResource,
        DeliverResourceToWorkplace
    }
    
    /// <summary>
    /// 
    /// </summary>
    public abstract class ResourceGathering : Task
    {
        protected ResourceType resourceType;
        protected AreaType[] gatherAreas;
        
        protected ResourceGatheringFlag currentGatheringState;
        protected ResourceToGatherBase resourceToGather;
        protected Vector3 resourcePosition;
        
        protected int gatheringSocketId;

        public Action<Resource> resourceDelivery;

        public abstract void DepleteCurrentResource();
    }
}
