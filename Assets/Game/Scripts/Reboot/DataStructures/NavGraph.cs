using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    [Serializable]
    public class AStarNode<T> : GraphNode<T>, IHeapItem<AStarNode<T>>
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }

        public bool IsOccupied { get; set; }

        public int HeapIndex { get; set; }

        public AStarNode<T> Previous;

        public AStarNode(T data) : base(data) { }

        public int CompareTo(AStarNode<T> other)
        {
            int compare = FCost.CompareTo(other.FCost);
            return -compare;
        }
    }
    [Serializable]
    public class NavGraph<T> : Graph<T>
    {
        public NavGraph() : base() { }

        public List<AStarNode<T>> GetPath(T from, T to, IHeuristic<T> heuristic)
        {
            AStarNode<T> fromNode = (AStarNode<T>)FindByValue(from);
            AStarNode<T> toNode = (AStarNode<T>)(FindByValue(to));

            if(fromNode == null || toNode == null)
            {
                return null;
            }

            return GetPath(fromNode, toNode, heuristic);
        }

        public override void AddNode(T value)
        {
            Nodes.Add(new AStarNode<T>(value));
        }

        public List<AStarNode<T>> GetPath(AStarNode<T> from, AStarNode<T> to, IHeuristic<T> heuristic)
        {
            Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(Nodes.Count);
            HashSet<AStarNode<T>> closedSet = new HashSet<AStarNode<T>>();

            foreach(AStarNode<T> node in Nodes)
            {
                node.HCost = 0;
                node.GCost = 0;
            }

            openSet.Add(from);

            while (openSet.Count > 0)
            {
                AStarNode<T> currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == to)
                {
                    return RetracePath(from, to);
                }

                foreach (AStarNode<T> neighbor in currentNode.Neighbors)
                {
                    if (neighbor == null || neighbor.IsOccupied || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float movementCost = currentNode.GCost + heuristic.NeighborDistance(currentNode.Data, neighbor.Data);

                    if (movementCost < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = movementCost;
                        neighbor.HCost = heuristic.Heuristic(currentNode.Data, to.Data);
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
        public List<AStarNode<T>> GetNodesInRange(T from, int range)
        {
            AStarNode<T> fromNode = (AStarNode<T>)FindByValue(from);
            return GetNodesInRange(fromNode, range);
        }

        private List<AStarNode<T>> GetNodesInRange(AStarNode<T> from, int range)
        {
            List<AStarNode<T>> nodesInRange = new List<AStarNode<T>>();

            Recursive(from, range, ref nodesInRange);

            return nodesInRange;
        }

        private void Recursive(AStarNode<T> from, int range, ref List<AStarNode<T>> nodesInRange)
        {
            if (range >= 0)
            {
                if(!nodesInRange.Contains(from))
                {
                    nodesInRange.Add(from);
                }
                foreach (AStarNode<T> neighbor in from.Neighbors)
                {
                    Recursive(neighbor, range - 1, ref nodesInRange);
                }
            }
        }
    }
}