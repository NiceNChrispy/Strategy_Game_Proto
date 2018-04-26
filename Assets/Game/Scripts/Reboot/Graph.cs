using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    [System.Serializable]
    public class Vertex<T>
    {
        public T Value { get; set; }

        public Vertex(T value)
        {
            Value = value;
        }
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

    [System.Serializable]
    public class Edge<T>
    {
        public Vertex<T> vertex0, vertex1;

        public Edge(Vertex<T> vertex0, Vertex<T> vertex1)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
        }

        static public bool operator ==(Edge<T> a, Edge<T> b)
        {
            return (a.vertex0.Value.Equals(b.vertex0.Value) && a.vertex1.Value.Equals(b.vertex1)) || (a.vertex0.Value.Equals(b.vertex1.Value) && a.vertex1.Value.Equals(b.vertex0.Value));
        }

        static public bool operator !=(Edge<T> a, Edge<T> b)
        {
            return !(a == b);
        }

        public bool Contains(Vertex<T> vertex)
        {
            return (vertex0.Value.Equals(vertex.Value) || vertex1.Value.Equals(vertex.Value));
        }
    }

    [System.Serializable]
    public class Graph<T>
    {
        public List<Vertex<T>> Vertices;
        public List<Edge<T>> Edges;

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
            Edges = new List<Edge<T>>();
        }

        public bool ContainsNode(Vertex<T> vertex)
        {
            return Vertices.Contains(vertex);
        }

        public bool Add(Vertex<T> vertex)
        {
            if (Vertices.Contains(vertex))
            {
                return false;
            }
            Vertices.Add(vertex);
            return true;
        }

        public void AddPair(Vertex<T> vertex0, Vertex<T> vertex1)
        {
            if(!Vertices.Contains(vertex0))
            {
                Vertices.Add(vertex0);
            }

            if(!Vertices.Contains(vertex1))
            {
                Vertices.Add(vertex1);
            }

            AddEdge(vertex0, vertex1);
        }

        public bool AddEdge(Vertex<T> vertex0, Vertex<T> vertex1)
        {
            if (vertex0 != vertex1 && Vertices.Contains(vertex0) && Vertices.Contains(vertex1))
            {
                Edge<T> newEdge = new Edge<T>(vertex0, vertex1);
                if (!Edges.Contains(newEdge))
                {
                    Edges.Add(newEdge);
                    return true;
                }
            }
            return false;
        }

        public void Remove(Vertex<T> vertex)
        {
            if (Vertices.Remove(vertex))
            {
                Edges.RemoveAll(x => x.Contains(vertex));
            }
        }

        public List<Vertex<T>> GetConnected(Vertex<T> vertex)
        {
            List<Vertex<T>> vertices = new List<Vertex<T>>();
            foreach(Edge<T> edge in Edges)
            {
                if(edge.Contains(vertex))
                {
                    if(!edge.vertex0.Value.Equals(vertex.Value))
                    {
                        vertices.Add(edge.vertex0);
                    }
                    if (!edge.vertex1.Value.Equals(vertex.Value))
                    {
                        vertices.Add(edge.vertex1);
                    }
                }
            }
            return vertices;
        }
    }
}