using System;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Resources;
using HopeMain.Code.World.Resources.ResourceToGather;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Tasks
{
    public enum Task_ResourceGathering_State
    {
        GO_TO_WORKPLACE,
        FIND_CLOSEST_RESOURCE,
        GO_TO_RESOURCE,
        GATHER_RESOURCE,
        DELIVER_RESOURCE_TO_WORKPLACE
    }
    
    public abstract class Task_ResourceGathering : Task
    {
        protected ResourceType resourceType;
        protected AreaType[] gatherAreas;
        
        protected Task_ResourceGathering_State currentGatheringState;
        protected ResourceToGather resourceToGather;
        protected Vector3 resourcePosition;
        
        protected int gatheringSocketId;

        public Action<Resource> onResourceDelivery;

        public abstract void DepleteCurrentResource();
    }
}
