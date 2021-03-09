using System;

namespace Code.Villagers.AI
{
    public enum NodeState
    {
        SUCCESS, RUNNING, FAILURE
    }
    
    [Serializable]
    public abstract class Node
    {
        protected NodeState state;

        public abstract NodeState Evaluate();

        public NodeState State => state;
    }
}
