using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class AStarVertex<T> : IHeapItem<AStarVertex<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public int HeapIndex { get; set; }

        public Vertex<T> Vertex;
        public AStarVertex<T> Previous;

        public AStarVertex(Vertex<T> vertex)
        {
            this.Vertex = vertex;
        }

        public int CompareTo(AStarVertex<T> other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }
    }

    public class AStarPathFinder<T> : PathFinder<T>
    {
        List<AStarVertex<T>> m_Nodes;

        public AStarPathFinder(Graph<T> graph) : base(graph)
        {
            m_Nodes = new List<AStarVertex<T>>();

            foreach (Vertex<T> vertex in m_Graph.Vertices)
            {
                m_Nodes.Add(new AStarVertex<T>(vertex));
            }
        }

        public override List<Vertex<T>> GetPath(Vertex<T> from, Vertex<T> to, Func<T, T, float> distanceFunction)
        {
            Heap<AStarVertex<T>> openSet = new Heap<AStarVertex<T>>(m_Graph.Vertices.Count);
            HashSet<AStarVertex<T>> closedSet = new HashSet<AStarVertex<T>>();

            openSet.Add(m_Nodes.Single(x => x.Vertex.Equals(from)));

            while (openSet.Count > 0)
            {
                AStarVertex<T> currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode.Vertex == to)
                {
                    AStarVertex<T> fromVertex = m_Nodes.Single(x => x.Vertex.Equals(from));
                    AStarVertex<T> toVertex = m_Nodes.Single(x => x.Vertex.Equals(to));
                    return RetracePath(fromVertex, toVertex);
                }

                List<Vertex<T>> vertexConnections = m_Graph.GetConnected(currentNode.Vertex);
                AStarVertex<T>[] connections = new AStarVertex<T>[vertexConnections.Count];
                for (int i = 0; i < vertexConnections.Count; i++)
                {
                    AStarVertex<T> vertex = m_Nodes.Single(x => x.Vertex.Equals(vertexConnections[i]));
                    if (vertex != null)
                    {
                        connections[i] = vertex;
                    }
                    else
                    {
                        Debug.LogError("NULL CONNECTION");
                    }
                }

                foreach (AStarVertex<T> connectedNode in connections)
                {
                    if (connectedNode == null || closedSet.Contains(connectedNode))
                    {
                        continue;
                    }

                    float movementCost = currentNode.GCost + distanceFunction(currentNode.Vertex.Value, connectedNode.Vertex.Value);

                    if (movementCost < connectedNode.GCost || !openSet.Contains(connectedNode))
                    {
                        connectedNode.GCost = movementCost;
                        connectedNode.HCost = + distanceFunction(currentNode.Vertex.Value, to.Value);
                        connectedNode.Previous = currentNode;
                        if (!openSet.Contains(connectedNode))
                        {
                            openSet.Add(connectedNode);
                        }
                        else
                        {
                            openSet.UpdateItem(connectedNode);
                        }
                    }
                }
            }
            return null;
        }

        List<Vertex<T>> RetracePath(AStarVertex<T> start, AStarVertex<T> end)
        {
            List<AStarVertex<T>> path = new List<AStarVertex<T>>();
            AStarVertex<T> currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.Previous;
            }

            path.Add(start);
            path.Reverse();

            List<Vertex<T>> vertices = new List<Vertex<T>>();

            foreach (AStarVertex<T> node in path)
            {
                vertices.Add(node.Vertex);
            }
            return vertices;
        }
    }
}