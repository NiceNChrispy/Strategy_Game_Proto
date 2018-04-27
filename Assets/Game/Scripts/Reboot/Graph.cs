using System.Collections.Generic;

namespace Reboot
{
    public class Vertex<T>
    {
        public T Data { get; set; }

        static public bool operator ==(Vertex<T> a, Vertex<T> b)
        {
            return (a.Data.Equals(b.Data));
        }

        static public bool operator !=(Vertex<T> a, Vertex<T> b)
        {
            return !(a == b);
        }
    }

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
            return ((a.vertex0 == b.vertex0 && a.vertex1 == b.vertex1) || (a.vertex0 == b.vertex1 && a.vertex1 == b.vertex0));
        }

        static public bool operator !=(Edge<T> a, Edge<T> b)
        {
            return !(a == b);
        }

        public bool Contains(Vertex<T> vertex)
        {
            return (vertex0 == vertex || vertex1== vertex);
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
                    if(!edge.vertex0.Data.Equals(vertex.Data))
                    {
                        vertices.Add(edge.vertex0);
                    }
                    if (!edge.vertex1.Data.Equals(vertex.Data))
                    {
                        vertices.Add(edge.vertex1);
                    }
                }
            }
            return vertices;
        }
    }
}