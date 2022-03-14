using System;
using UnityEngine;

namespace _Prototype.Code.v001.World.Resources
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum ResourceType
    {
        Wood, Stone
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Resource
    {
        [SerializeField] private ResourceType type;

        public int amount;
        private int _limit;

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
            _limit = limit;
        }

        public ResourceType Type => type;

        public int Limit => _limit;
    }
}
