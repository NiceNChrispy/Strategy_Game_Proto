//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Reboot
//{
//    public class AStarVertex<T> : IVertex<T>, IHeapItem<AStarVertex<T>>
//    {
//        public float GCost { get; set; }
//        public float HCost { get; set; }
//        public float FCost { get { return GCost + HCost; } }

//        public bool IsOccupied { get; set; }

//        public int HeapIndex { get; set; }

//        public T Value
//        {
//            get; set;
//        }

//        public AStarVertex<T> Previous;

//        public AStarVertex(T value)
//        {
//            this.Value = value;
//        }

//        public int CompareTo(AStarVertex<T> other)
//        {
//            int compare = FCost.CompareTo(other.FCost);
//            if (compare == 0)
//            {
//                compare = HCost.CompareTo(other.HCost);
//            }
//            return -compare;
//        }
//    }

//    public class NavGraph<T> : Graph<T>
//    {
//        public NavGraph() : base() { }

//        public List<IVertex<T>> GetPath(IVertex<T> from, IVertex<T> to, Func<T, T, float> distanceFunction)
//        {
//            Heap<AStarVertex<T>> openSet = new Heap<AStarVertex<T>>(base.Vertices.Count);
//            HashSet<AStarVertex<T>> closedSet = new HashSet<AStarVertex<T>>();
//            Debug.Log(base.Vertices.Count);

//            openSet.Add(Vertices.Single(x => x.Value.Equals(from.Value)));

//            while (openSet.Count > 0)
//            {
//                AStarVertex<T> currentNode = openSet.RemoveFirst();

//                closedSet.Add(currentNode);

//                if (currentNode.Value.Equals(to.Value))
//                {
//                    AStarVertex<T> fromVertex = Vertices.Single(x => x.Value.Equals(from.Value));
//                    AStarVertex<T> toVertex = Vertices.Single(x => x.Value.Equals(to.Value));
//                    return RetracePath(fromVertex, toVertex);
//                }

//                List<IVertex<T>> vertexConnections = GetConnected(currentNode);
//                AStarVertex<T>[] connections = new AStarVertex<T>[vertexConnections.Count];
//                for (int i = 0; i < vertexConnections.Count; i++)
//                {
//                    AStarVertex<T> vertex = Vertices.Single(x => x.Value.Equals(vertexConnections[i].Value));
//                    if (vertex != null)
//                    {
//                        connections[i] = vertex;
//                    }
//                    else
//                    {
//                        Debug.LogError("NULL CONNECTION");
//                    }
//                }

//                foreach (AStarVertex<T> connectedNode in connections)
//                {
//                    if (connectedNode == null || closedSet.Contains(connectedNode))
//                    {
//                        continue;
//                    }

//                    float movementCost = currentNode.GCost + distanceFunction(currentNode.Value, connectedNode.Value);

//                    if (!connectedNode.IsOccupied && movementCost < connectedNode.GCost || !openSet.Contains(connectedNode))
//                    {
//                        connectedNode.GCost = movementCost;
//                        connectedNode.HCost += distanceFunction(currentNode.Value, to.Value);
//                        connectedNode.Previous = currentNode;
//                        if (!openSet.Contains(connectedNode))
//                        {
//                            openSet.Add(connectedNode);
//                        }
//                        else
//                        {
//                            openSet.UpdateItem(connectedNode);
//                        }
//                    }
//                }
//            }
//            return null;
//        }

//        List<IVertex<T>> RetracePath(AStarVertex<T> start, AStarVertex<T> end)
//        {
//            List<AStarVertex<T>> path = new List<AStarVertex<T>>();
//            AStarVertex<T> currentNode = end;

//            while (currentNode != start)
//            {
//                path.Add(currentNode);
//                currentNode = currentNode.Previous;
//            }

//            path.Add(start);
//            path.Reverse();

//            List<IVertex<T>> vertices = new List<IVertex<T>>();

//            foreach (AStarVertex<T> node in path)
//            {
//                vertices.Add(node);
//            }
//            return vertices;
//        }
//    }
//}