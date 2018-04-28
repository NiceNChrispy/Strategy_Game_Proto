using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    [Serializable]
    public class Graph<T> : IEnumerable<T>
    {
        public List<Node<T>> Nodes { get; protected set; }

        public Graph() : this(new List<Node<T>>()) { }
        public Graph(List<Node<T>> nodes)
        {
            Nodes = nodes;
        }

        public Node<T> FindByValue(T value)
        {
            foreach (Node<T> node in Nodes)
            {
                if (node.Data.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }

        public virtual void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            Nodes.Add(node);
        }

        public virtual void AddNode(T value)
        {
            // adds a node to the graph
            Nodes.Add(new GraphNode<T>(value));
        }

        public virtual void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public virtual void AddDirectedEdge(T fromValue, T toValue, int cost)
        {
            GraphNode<T> fromNode = (GraphNode<T>)FindByValue(fromValue);
            GraphNode<T> toNode = (GraphNode<T>)FindByValue(toValue);

            if (fromNode != null && toNode != null)
            {
                fromNode.Neighbors.Add(toNode);
                fromNode.Costs.Add(cost);
            }
        }

        public virtual void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public virtual void AddUndirectedEdge(T fromValue, T toValue, int cost)
        {
            GraphNode<T> fromNode = (GraphNode<T>)FindByValue(fromValue);
            GraphNode<T> toNode = (GraphNode<T>)FindByValue(toValue);

            if (fromNode != null && toNode != null)
            {
                fromNode.Neighbors.Add(toNode);
                fromNode.Costs.Add(cost);

                toNode.Neighbors.Add(fromNode);
                toNode.Costs.Add(cost);
            }
        }

        public bool Contains(T value)
        {
            return FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)FindByValue(value);
            if (nodeToRemove == null)
            {
                return false;
            }
            // node wasn't found

            // otherwise, the node was found
            Nodes.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> node in Nodes)
            {
                int index = node.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    node.Neighbors.RemoveAt(index);
                    node.Costs.RemoveAt(index);
                }
            }
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return NodeValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> NodeValues()
        {
            foreach (Node<T> node in Nodes)
            {
                yield return node.Data;
            }
        }

        public int Count
        {
            get { return Nodes.Count; }
        }
    }
}