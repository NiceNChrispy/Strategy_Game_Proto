using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public interface IVertex<T>
    {
        T Value { get; set; }
    }

    //public class Edge
    //{
    //    public int index0, index1;

    //    public Edge(int index0, int index1)
    //    {
    //        this.index0 = index0;
    //        this.index1 = index1;
    //    }

    //    static public bool operator ==(Edge a, Edge b)
    //    {
    //        return (a.index0 == b.index0 && a.index1 == b.index1) || (a.index0 == b.index1 && a.index1 == b.index0);
    //    }

    //    static public bool operator !=(Edge a, Edge b)
    //    {
    //        return !(a == b);
    //    }

    //    public bool Contains(int index)
    //    {
    //        return (index0 == index || index1 == index);
    //    }
    //}

    public class Edge<T>
    {
        public IVertex<T> vertex0, vertex1;

        public Edge(IVertex<T> vertex0, IVertex<T> vertex1)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
        }

        static public bool operator ==(Edge<T> a, Edge<T> b)
        {
            return (a.vertex0 == b.vertex0 && a.vertex1 == b.vertex1) || (a.vertex0 == b.vertex1 && a.vertex1 == b.vertex0);
        }

        static public bool operator !=(Edge<T> a, Edge<T> b)
        {
            return !(a == b);
        }

        public bool Contains(IVertex<T> vertex)
        {
            return (vertex0 == vertex || vertex1 == vertex);
        }
    }

    public class Graph<T>
    {
        List<IVertex<T>> m_Vertices;
        List<Edge<T>> m_Edges;

        public List<Edge<T>> Edges
        {
            get
            {
                return m_Edges;
            }
        }
        public List<IVertex<T>> Vertices
        {
            get
            {
                return m_Vertices;
            }
        }

        public Graph()
        {
            m_Vertices = new List<IVertex<T>>();
            m_Edges = new List<Edge<T>>();
        }

        public bool ContainsNode(IVertex<T> node)
        {
            return m_Vertices.Contains(node);
        }

        public bool ContainsNodeWithData(T data)
        {
            return m_Vertices.Any(x => x.Value.Equals(data));
        }

        public bool Add(IVertex<T> node)
        {
            if (m_Vertices.Contains(node))
            {
                return false;
            }
            m_Vertices.Add(node);
            return true;
        }

        public bool AddEdge(IVertex<T> nodeA, IVertex<T> nodeB)
        {
            if (nodeA != nodeB && m_Vertices.Contains(nodeA) && m_Vertices.Contains(nodeB))
            {
                Edge<T> newEdge = new Edge<T>(nodeA, nodeB);
                if (!m_Edges.Contains(newEdge))
                {
                    m_Edges.Add(newEdge);
                    return true;
                }
            }
            return false;
        }

        public void Remove(IVertex<T> node)
        {
            if (m_Vertices.Remove(node))
            {
                m_Edges.RemoveAll(x => x.Contains(node));
            }
        }
    }
}