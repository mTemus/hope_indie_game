using System;
using UnityEngine;

namespace Code.Map.Resources
{
    [Serializable]
    public enum ResourceType
    {
        WOOD, STONE
    }

    [Serializable]
    public class Resource
    {
        [SerializeField] private ResourceType type;

        public int amount;
        private int limit;

        public Resource(Resource copy)
        {
            type = copy.type;
            amount = copy.amount;
        }
        
        public Resource(ResourceType type)
        { 
            this.type = type;
        }

        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public Resource(ResourceType type, int amount, int limit)
        {
            this.type = type;
            this.amount = amount;
            this.limit = limit;
        }

        public ResourceType Type => type;

        public int Limit => limit;
    }
}
