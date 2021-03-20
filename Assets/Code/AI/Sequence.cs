using System;
using System.Collections.Generic;

namespace Code.AI
{
    public class Sequence : Node
    {
        private readonly List<Node> nodes;
        public Sequence(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool isAnyNodeRunning = false;
        
            foreach (Node node in nodes) {
                switch (node.Evaluate()) {
                    case NodeState.SUCCESS:
                        break;
                
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            state = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
