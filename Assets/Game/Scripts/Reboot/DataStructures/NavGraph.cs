using System;
using System.Collections.Generic;

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
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }
    }
    [Serializable]
    public class NavGraph<T> : Graph<T>
    {
        public NavGraph() : base() { }

        public List<AStarNode<T>> GetPath(T from, T to, Func<T, T, float> distanceFunction)
        {
            AStarNode<T> fromNode = (AStarNode<T>)FindByValue(from);
            AStarNode<T> toNode = (AStarNode<T>)(FindByValue(to));
            return GetPath(fromNode, toNode, distanceFunction);
        }

        public override void AddNode(T value)
        {
            Nodes.Add(new AStarNode<T>(value));
        }

        public List<AStarNode<T>> GetPath(AStarNode<T> from, AStarNode<T> to, Func<T, T, float> distanceFunction)
        {
            Heap<AStarNode<T>> openSet = new Heap<AStarNode<T>>(Nodes.Count);
            HashSet<AStarNode<T>> closedSet = new HashSet<AStarNode<T>>();

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

                    float movementCost = currentNode.GCost + distanceFunction(currentNode.Data, neighbor.Data);

                    if (movementCost < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = movementCost;
                        neighbor.HCost = distanceFunction(currentNode.Data, to.Data);
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