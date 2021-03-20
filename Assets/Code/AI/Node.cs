using System;

namespace Code.AI
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
