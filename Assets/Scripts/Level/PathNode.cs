using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
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

    public PathNode Parent;

    public int CompareTo(PathNode other)
    {
        //int compare = FCost.CompareTo(other.FCost);
        //if (compare == 0)
        //{
            int compare = HCost.CompareTo(other.HCost);
        //}
        return -compare;
    }
}