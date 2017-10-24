using UnityEngine;

namespace Navigation
{
    public class Node : IHeapItem<Node>
    {
        public Vector3 Position;
        public float GCost;
        public float HCost;
        public bool IsTraversible;

        public float FCost { get { return GCost + HCost; } }
        public int HeapIndex
        {
            get;
            set;
        }

        public Node Parent;

        public int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }
    }
}