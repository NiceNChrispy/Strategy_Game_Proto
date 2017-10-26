using System;
using System.Collections.Generic;

namespace Navigation
{
    [Serializable]
    public class Node<T> : IHeapItem<Node<T>> where T : IComparable<T>
    {
        public T Data;
        public int HeapIndex
        {
            get;
            set;
        }

        public List<Node<T>> Connections
        {
            get
            {
                return connections;
            }

            set
            {
                connections = value;
            }
        }
        private List<Node<T>> connections;

        public Node(T data)
        {
            CheckConnections();
            Data = data;
        }

        void CheckConnections()
        {
            if(Connections == null)
            {
                Connections = new List<Node<T>>();
            }
        }

        public virtual void AddConnection(Node<T> connection)
        {
            Connections.Add(connection);
        }

        public virtual void AddConnections(List<Node<T>> connections)
        {
            Connections.AddRange(connections);
        }

        public int CompareTo(Node<T> other)
        {
            return Data.CompareTo(other.Data);
        }
    }
}