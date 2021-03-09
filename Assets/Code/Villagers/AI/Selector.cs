using System;
using System.Collections.Generic;

namespace Code.Villagers.AI
{
    public class Selector : Node
    {
        private readonly List<Node> nodes;
        public Selector(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach (Node node in nodes) {
                switch (node.Evaluate()) {
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                
                    case NodeState.FAILURE:
                        break;
                
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
