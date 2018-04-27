using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class AStarVertex<T> : Vertex<T>, IHeapItem<AStarVertex<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public bool IsOccupied { get; set; }

        public int HeapIndex { get; set; }

        public T Value
        {
            get; set;
        }

        public AStarVertex<T> Previous;

        public AStarVertex(T value)
        {
            this.Value = value;
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

    public class AStarNavGraph<T> : NavGraph<T>
    {
        List<AStarVertex<T>> m_Nodes;

        public AStarNavGraph()
        {
            m_Nodes = new List<AStarVertex<T>>();

            foreach (AStarVertex<T> vertex in Vertices)
            {
                m_Nodes.Add(new AStarVertex<T>(vertex.Value));
            }
        }

        public override List<Vertex<T>> GetPath(Vertex<T> from, Vertex<T> to, Func<T, T, float> distanceFunction)
        {
            Heap<AStarVertex<T>> openSet = new Heap<AStarVertex<T>>(Vertices.Count);
            HashSet<AStarVertex<T>> closedSet = new HashSet<AStarVertex<T>>();

            openSet.Add(m_Nodes.Single(x => x.Value.Equals(from.Data)));

            while (openSet.Count > 0)
            {
                AStarVertex<T> currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode.Value.Equals(to.Data))
                {
                    AStarVertex<T> fromVertex = m_Nodes.Single(x => x.Value.Equals(from.Data));
                    AStarVertex<T> toVertex = m_Nodes.Single(x => x.Value.Equals(to.Data));
                    return RetracePath(fromVertex, toVertex);
                }

                List<Vertex<T>> vertexConnections = GetConnected(currentNode);
                AStarVertex<T>[] connections = new AStarVertex<T>[vertexConnections.Count];
                for (int i = 0; i < vertexConnections.Count; i++)
                {
                    AStarVertex<T> vertex = m_Nodes.Single(x => x.Value.Equals(vertexConnections[i].Data));
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

                    float movementCost = currentNode.GCost + distanceFunction(currentNode.Value, connectedNode.Value);

                    if (!connectedNode.IsOccupied && movementCost < connectedNode.GCost || !openSet.Contains(connectedNode))
                    {
                        connectedNode.GCost = movementCost;
                        connectedNode.HCost = + distanceFunction(currentNode.Value, to.Data);
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
                vertices.Add(node);
            }
            return vertices;
        }
    }
}