using Navigation;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AStarData : IComparable<AStarData>
{
    public float GCost;
    public float HCost;
    public float FCost { get { return GCost + HCost; } }

    public AStarNode Parent { get; internal set; }

    public bool IsTraversible;
    public Vector3 Position;

    public int CompareTo(AStarData other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }
        return -compare;
    }
}

public class AStarNode : Node<AStarData>, IComparable<AStarNode>, IHeapItem<AStarNode>
{
    public AStarNode(AStarData data) : base(data) { }

    public int CompareTo(AStarNode other)
    {
        return Data.CompareTo(other.Data);
    }

    internal void AddConnections(List<AStarNode> list)
    {
        AddConnections(list);
    }
}