using System;
using System.Collections.Generic;
using Code.Villagers.AI;

public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();
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
