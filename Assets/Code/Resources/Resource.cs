using System;
using UnityEngine;

namespace Code.Resources
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
        
        public Resource(ResourceType type)
        { 
            this.type = type;
        }

        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public ResourceType Type => type;
    }
}
