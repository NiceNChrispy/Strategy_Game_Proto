using Reboot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataStructures
{
    [Serializable]
    public class AStarNode<T> : INavNode<T>, IHeapItem<AStarNode<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public bool IsTraversible { get; set; }

        public int HeapIndex { get; set; }

        public T Data { get; set; }

        public float Cost { get; set; }
        public List<INavNode<T>> Connected { get; set; }

        public AStarNode<T> Previous;

        public AStarNode(T data, bool isTraversible, float cost)
        {
            Data = data;
            IsTraversible = isTraversible;
            Cost = cost;
            Connected = new List<INavNode<T>>();
        }

        public int CompareTo(AStarNode<T> other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }
    }
    [Serializable]
    public class NavGraph<T>
    {
        List<AStarNode<T>> m_Nodes = new List<AStarNode<T>>();

        public List<AStarNode<T>> Nodes { get { return m_Nodes; } private set { m_Nodes = value; } }

        public List<AStarNode<T>> GetPath(T from, T to, IHeuristic<T> heuristic)
        {
            AStarNode<T> fromNode = m_Nodes.SingleOrDefault(x => x.Data.Equals(from));
            AStarNode<T> toNode = m_Nodes.SingleOrDefault(x => x.Data.Equals(to));

            if (fromNode == null || toNode == null)
            {
                return null;
            }

            return GetPath(fromNode, toNode, heuristic);
        }

        public List<AStarNode<T>> GetPath(AStarNode<T> from, AStarNode<T> to, IHeuristic<T> heuristic)
        {
            Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(Nodes.Count);
            HashSet<AStarNode<T>> closedSet = new HashSet<AStarNode<T>>();

            openSet.Add(from);

            while (openSet.Count > 0)
            {
                AStarNode<T> currentNode = openSet.RemoveFirst();

                if (currentNode == to)
                {
                    return RetracePath(from, to);
                }

                closedSet.Add(currentNode);

                foreach (AStarNode<T> neighbor in currentNode.Connected)
                {
                    if (!neighbor.IsTraversible || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float movementCost = currentNode.GCost + heuristic.NeighborDistance(currentNode.Data, neighbor.Data);

                    if (movementCost < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = movementCost;
                        neighbor.HCost = heuristic.Heuristic(neighbor.Data, to.Data);
                        neighbor.Previous = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
            return null;
        }

        List<AStarNode<T>> RetracePath(AStarNode<T> start, AStarNode<T> end)
        {
            List<AStarNode<T>> path = new List<AStarNode<T>>();
            AStarNode<T> currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.Previous;
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}
