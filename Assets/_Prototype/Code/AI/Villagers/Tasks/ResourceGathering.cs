using System;
using _Prototype.Code.World.Areas;
using _Prototype.Code.World.Resources;
using _Prototype.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace _Prototype.Code.AI.Villagers.Tasks
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
