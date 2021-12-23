using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Planilo.BT
{

    /// <summary>Class to represent a behavior tree as an AIGraph.</sumamry>
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "BT/BehaviorTree", order = 1)]
    public class BTGraph : AIGraph
    {
        /// <summary>Root node of the behavior tree.</summary>
        [SerializeField] private BTBranchNode _root;
        public BTBranchNode root => _root;

        /// <summary>Sets the root node of the behavior tree.</summary>
        public void SetRoot(BTBranchNode node)
        {
            // Unset previous root.
            if (_root != null && nodes.Contains(_root))
                _root.IsRoot = false;

            // Set new root.
            if(nodes.Contains(node))
                _root = node;
        }

        /// <summary>Unsets the root node of the behavior tree if the given node matches the root.</summary>
        public void UnsetRoot(BTBranchNode node)
        {
            // Only unset when the state passed is the same as the current one.
            if (node == _root)
                _root = null;
        }

        public override NodeGraph Copy()
        {
            // Instantiate a new nodegraph instance
            BTGraph graph = Instantiate(this);

            // Instantiate all nodes inside the graph
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == null) continue;
                Node.graphHotfix = graph;
                Node node = Instantiate(nodes[i]);
                node.graph = graph;

                // replace (Clone) in node name to avoid issues with blackboard
                node.name = node.name.Replace("(Clone)", "");
                graph.nodes[i] = node;

                // handling for setting root
                BTBranchNode originalNode = nodes[i] as BTBranchNode;
                if (originalNode != null && originalNode.IsRoot)
                {
                    graph.UnsetRoot(_root);
                    graph.SetRoot(node as BTBranchNode);
                }
            }

            // Redirect all connections
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                if (graph.nodes[i] == null) continue;
                foreach (NodePort port in graph.nodes[i].Ports)
                    port.Redirect(nodes, graph.nodes);
            }

            return graph;
        }

        /// <summary>Initialize the behavior tree and the blackboard.</summary>
        public override void Init(AIBlackboard blackboard)
        {
            // Save blackboard while executing.
            _blackboard = blackboard;
            // Init all nodes.
            foreach (Node node in nodes)
            {
                BTNode btNode = node as BTNode;
                // If node is of BTNode type prepare it for execution.
                if (btNode != null)
                {
                    btNode.Prepare();
                }
            }
        }

        /// <summary>Execute the behavior tree in the blackboard.</summary>
        public override AIGraphResult Run(AIBlackboard blackboard)
        {
            // Save blackboard while executing.
            _blackboard = blackboard;
            // Execute root.
            BTGraphResult result = _root.Run();
            // Nullify references to the blackboard.
            _blackboard = null;
            // Return result.
            return result;
        }
    }
}