using UnityEngine;

namespace Navigation
{
    public class NavNode : IHeapItem<NavNode>
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

        public NavNode Parent;

        public int CompareTo(NavNode other)
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