using System;
using System.Collections.Generic;

namespace DataStructures
{
    [Serializable]
    public class GraphNode<T> : Node<T>
    {
        public List<int> Costs { get; set; }

        public GraphNode(T data) : base(data)
        {
            Costs = new List<int>();
        }
    }
}