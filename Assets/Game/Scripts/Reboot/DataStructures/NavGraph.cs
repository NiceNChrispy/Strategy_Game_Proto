using Reboot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataStructures
{
    [Serializable]
    public class AStarNode<T> : NavNode<T>, IHeapItem<AStarNode<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public int HeapIndex { get; set; }

        public AStarNode<T> Previous;

        public AStarNode(T data, bool isTraversible, float cost) : base (data, isTraversible, cost) { }

        public void Reset()
        {
            GCost = 0;
            HCost = 0;
            Previous = null;
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

        public void ResetNodes()
        {
            foreach(AStarNode<T> node in m_Nodes)
            {
                node.Reset();
            }
        }

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
            ResetNodes();

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

        public List<AStarNode<T>> GetNodesInRange(T from, int range, IHeuristic<T> heuristic)
        {
            AStarNode<T> fromNode = m_Nodes.SingleOrDefault(x => x.Data.Equals(from));
            return GetNodesInRange(fromNode, range, heuristic);
        }

        private List<AStarNode<T>> GetNodesInRange(AStarNode<T> from, int range, IHeuristic<T> heuristic)
        {
            List<AStarNode<T>> nodesInRange = new List<AStarNode<T>>();
            Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(Nodes.Count);
            HashSet<AStarNode<T>> closedSet = new HashSet<AStarNode<T>>();

            openSet.Add(from);

            ResetNodes();

            while (openSet.Count > 0)
            {
                AStarNode<T> node = openSet.RemoveFirst();
                nodesInRange.Add(node);
                closedSet.Add(node);

                foreach (AStarNode<T> connected in node.Connected)
                {
                    if (!connected.IsTraversible || closedSet.Contains(connected))
                    {
                        continue;
                    }
                    int cost = Mathf.RoundToInt(node.GCost + heuristic.Heuristic(node.Data, connected.Data));

                    if (cost < connected.GCost || !openSet.Contains(connected))
                    {
                        connected.GCost = cost;
                        if (!openSet.Contains(connected))
                        {
                            openSet.Add(connected);
                        }
                        else
                        {
                            openSet.UpdateItem(connected);
                        }
                    }
                }
            }

            return nodesInRange.Where(x => x.GCost <= range).ToList();
        }
    }
}
