//using System.Collections.Generic;

//namespace Reboot
//{
//    public interface IVertex<T>
//    {
//        T Value { get; set; }
//    }

//    [System.Serializable]
//    public class Edge<T>
//    {
//        public IVertex<T> vertex0, vertex1;

//        public Edge(IVertex<T> vertex0, IVertex<T> vertex1)
//        {
//            this.vertex0 = vertex0;
//            this.vertex1 = vertex1;
//        }

//        static public bool operator ==(Edge<T> a, Edge<T> b)
//        {
//            return (a.vertex0.Value.Equals(b.vertex0.Value) && a.vertex1.Value.Equals(b.vertex1)) || (a.vertex0.Value.Equals(b.vertex1.Value) && a.vertex1.Value.Equals(b.vertex0.Value));
//        }

//        static public bool operator !=(Edge<T> a, Edge<T> b)
//        {
//            return !(a == b);
//        }

//        public bool Contains(IVertex<T> vertex)
//        {
//            return (vertex0.Value.Equals(vertex.Value) || vertex1.Value.Equals(vertex.Value));
//        }
//    }

//    [System.Serializable]
//    public class Graph<T>
//    {
//        public List<IVertex<T>> Vertices;
//        public List<Edge<T>> Edges;

//        public Graph()
//        {
//            Vertices = new List<IVertex<T>>();
//            Edges = new List<Edge<T>>();
//        }

//        public bool ContainsNode(IVertex<T> vertex)
//        {
//            return Vertices.Contains(vertex);
//        }

//        public virtual bool Add(IVertex<T> vertex)
//        {
//            if (Vertices.Contains(vertex))
//            {
//                return false;
//            }
//            Vertices.Add(vertex);
//            return true;
//        }

//        public void AddPair(IVertex<T> vertex0, IVertex<T> vertex1)
//        {
//            if(!Vertices.Contains(vertex0))
//            {
//                Vertices.Add(vertex0);
//            }

//            if(!Vertices.Contains(vertex1))
//            {
//                Vertices.Add(vertex1);
//            }

//            AddEdge(vertex0, vertex1);
//        }

//        public bool AddEdge(IVertex<T> vertex0, IVertex<T> vertex1)
//        {
//            if (vertex0 != vertex1 && Vertices.Contains(vertex0) && Vertices.Contains(vertex1))
//            {
//                Edge<T> newEdge = new Edge<T>(vertex0, vertex1);
//                if (!Edges.Contains(newEdge))
//                {
//                    Edges.Add(newEdge);
//                    return true;
//                }
//            }
//            return false;
//        }

//        public void Remove(IVertex<T> vertex)
//        {
//            if (Vertices.Remove(vertex))
//            {
//                Edges.RemoveAll(x => x.Contains(vertex));
//            }
//        }

//        public List<IVertex<T>> GetConnected(IVertex<T> vertex)
//        {
//            List<IVertex<T>> vertices = new List<IVertex<T>>();
//            foreach(Edge<T> edge in Edges)
//            {
//                if(edge.Contains(vertex))
//                {
//                    if(!edge.vertex0.Value.Equals(vertex.Value))
//                    {
//                        vertices.Add(edge.vertex0);
//                    }
//                    if (!edge.vertex1.Value.Equals(vertex.Value))
//                    {
//                        vertices.Add(edge.vertex1);
//                    }
//                }
//            }
//            return vertices;
//        }
//    }
//}