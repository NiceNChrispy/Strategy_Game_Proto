using System;
using System.Collections.Generic;

namespace DataStructures
{
    [Serializable]
    public class Node<T>
    {
        public T Data { get; set; }

        public List<Node<T>> Neighbors { get; protected set; }

        public Node(T data)
        {
            Data = data;
            Neighbors = new List<Node<T>>();
        }
    }
}