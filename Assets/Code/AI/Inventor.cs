using System;

namespace Code.AI
{
    public class Inventor : Node
    {
        private readonly Node node;

        public Inventor(Node node)
        {
            this.node = node;
        }

        public override NodeState Evaluate()
        {
            state = node.Evaluate() switch {
                NodeState.SUCCESS => NodeState.FAILURE,
                NodeState.RUNNING => NodeState.RUNNING,
                NodeState.FAILURE => NodeState.SUCCESS,
                _ => throw new ArgumentOutOfRangeException()
            };

            return state;
        }
    }
}
