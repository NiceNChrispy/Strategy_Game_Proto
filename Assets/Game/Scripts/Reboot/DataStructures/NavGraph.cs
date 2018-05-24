using Reboot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataStructures
{
    [Serializable]
    public class AStarData<T> : IHeapItem<AStarData<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public int HeapIndex { get; set; }

        public AStarData<T> Previous;
        public INavNode<T> Node;

        public int CompareTo(AStarData<T> other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }
    }

    public class CostData<T> : IHeapItem<CostData<T>>
    {
        public int Cost { get; set; }
        public INavNode<T> Node { get; set; }
        public int HeapIndex { get; set; }

        public int CompareTo(CostData<T> other)
        {
            return other.Cost.CompareTo(Cost);
        }
    }
    [Serializable]
    public class NavGraph<T>
    {
        List<INavNode<T>> m_Nodes = new List<INavNode<T>>();

        public List<INavNode<T>> Nodes { get { return m_Nodes; } private set { m_Nodes = value; } }

        public List<INavNode<T>> GetPath(T from, T to, IHeuristic<T> heuristic)
        {
            INavNode<T> fromNode = m_Nodes.SingleOrDefault(x => x.Position.Equals(from));
            INavNode<T> toNode = m_Nodes.SingleOrDefault(x => x.Position.Equals(to));

            if (fromNode == null || toNode == null)
            {
                return null;
            }

            return GetPath(fromNode, toNode, heuristic);
        }

        public List<INavNode<T>> GetPath(INavNode<T> from, INavNode<T> to, IHeuristic<T> heuristic)
        {
            Heap<AStarData<T>> openSet = new Heap<AStarData<T>>(m_Nodes.Count);
            HashSet<AStarData<T>> closedSet = new HashSet<AStarData<T>>();

            AStarData<T>[] aStarData = new AStarData<T>[m_Nodes.Count];
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                aStarData[i] = new AStarData<T>
                {
                    Node = m_Nodes[i],
                    GCost = 0.0f,
                    HCost = 0.0f,
                    Previous = null
                };
            }

            openSet.Add(aStarData[m_Nodes.IndexOf(from)]);

            aStarData[m_Nodes.IndexOf(from)].Node = from;

            while (openSet.Count > 0)
            {
                AStarData<T> currentNode = openSet.RemoveFirst();
                if (currentNode.Node == to)
                {
                    return RetracePath(aStarData[m_Nodes.IndexOf(from)], aStarData[m_Nodes.IndexOf(to)]);
                }

                closedSet.Add(currentNode);

                int currentIndex = m_Nodes.IndexOf(currentNode.Node);

                foreach (INavNode<T> connectedNode in currentNode.Node.Connected)
                {
                    int connectedIndex = m_Nodes.IndexOf(connectedNode);
                    AStarData<T> connectedData = aStarData[connectedIndex];
                    if (!connectedNode.IsTraversible || closedSet.Contains(connectedData))
                    {
                        continue;
                    }

                    float movementCost = aStarData[currentIndex].GCost + heuristic.NeighborDistance(currentNode.Node.Position, connectedNode.Position);

                    if (movementCost < aStarData[connectedIndex].GCost || !openSet.Contains(connectedData))
                    {
                        aStarData[connectedIndex].GCost = movementCost;
                        aStarData[connectedIndex].HCost = heuristic.Heuristic(connectedNode.Position, to.Position);
                        aStarData[connectedIndex].Previous = aStarData[currentIndex];

                        if (!openSet.Contains(connectedData))
                        {
                            openSet.Add(connectedData);
                        }
                        else
                        {
                            openSet.UpdateItem(connectedData);
                        }
                    }
                }
            }
            return null;
        }

        List<INavNode<T>> RetracePath(AStarData<T> start, AStarData<T> end)
        {
            List<INavNode<T>> path = new List<INavNode<T>>();
            AStarData<T> currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode.Node);
                currentNode = currentNode.Previous;
            }

            path.Add(start.Node);
            path.Reverse();

            return path;
        }

        public List<INavNode<T>> GetNodesInRange(T from, int range, IHeuristic<T> heuristic)
        {
            INavNode<T> fromNode = m_Nodes.SingleOrDefault(x => x.Position.Equals(from));
            return GetNodesInRange(fromNode, range, heuristic);
        }

        private List<INavNode<T>> GetNodesInRange(INavNode<T> from, int range, IHeuristic<T> heuristic)
        {
            List<INavNode<T>> nodesInRange = new List<INavNode<T>>();
            Heap<CostData<T>> openSet = new Heap<CostData<T>>(m_Nodes.Count);
            HashSet<CostData<T>> closedSet = new HashSet<CostData<T>>();

            CostData<T>[] costData = new CostData<T>[m_Nodes.Count];
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                costData[i] = new CostData<T>
                {
                    Node = m_Nodes[i],
                    Cost = 0
                };
            }

            openSet.Add(costData[m_Nodes.IndexOf(from)]);

            while (openSet.Count > 0)
            {
                CostData<T> currentData = openSet.RemoveFirst();
                nodesInRange.Add(currentData.Node);
                closedSet.Add(currentData);

                int currentIndex = m_Nodes.IndexOf(currentData.Node);

                foreach (INavNode<T> connectedNode in currentData.Node.Connected)
                {
                    int connectedIndex = m_Nodes.IndexOf(connectedNode);
                    CostData<T> connectedData = costData[connectedIndex];
                    if (!connectedNode.IsTraversible || closedSet.Contains(connectedData))
                    {
                        continue;
                    }

                    int cost = currentData.Cost + 1;

                    if (cost < connectedData.Cost || !openSet.Contains(connectedData))
                    {
                        connectedData.Cost = cost;
                        if (!openSet.Contains(connectedData))
                        {
                            openSet.Add(connectedData);
                        }
                        else
                        {
                            openSet.UpdateItem(connectedData);
                        }
                    }
                }
            }

            return nodesInRange.Where(x => costData[m_Nodes.IndexOf(x)].Cost <= range).ToList();
        }
    }
}
