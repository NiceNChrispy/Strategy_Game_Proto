using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class NodeGrid : MonoBehaviour
{
    private PathNode[] m_PathNodes;
    [SerializeField] int m_Width, m_Height;

    PathNode[] m_Accessible;

    [SerializeField] private int xPos, yPos;
    [SerializeField] private int xPosEnd, yPosEnd;

    private PathNode this[int x, int y]
    {
        get
        {
            return (x >= 0 && x < m_Width && y >= 0 && y < m_Height) ? m_PathNodes[(x * m_Height) + y] : null;
        }

        set
        {
            m_PathNodes[(x * m_Height) + y] = value;
        }
    }

    private void Awake()
    {
        Create();
    }

    private void Update()
    {
        m_Accessible = GetPath(xPos, yPos, xPosEnd, yPosEnd).ToArray();
        //m_Accessible = GetAccessable(xPos, yPos, minRange, maxRange, isManhatten);

        //m_Accessible = m_TileNodes.Where(x =>
        //{
        //    float distance = isManhatten ? Mathf.Abs(x.Data.x - xPos) + Mathf.Abs(x.Data.z - yPos) : Vector2.Distance(new Vector2(xPos, yPos), new Vector2(x.Data.x, x.Data.z));
        //    return (distance >= minRange && distance <= maxRange);
        //}
        //).ToArray();
    }

    public void Create()
    {
        m_PathNodes = new PathNode[m_Width * m_Height];

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                this[x, y] = new PathNode();
                this[x, y].Position = new Vector3(x, 1, y);
                this[x, y].IsTraversible = true;

            }
        }
    }

    //public void Connect()
    //{
    //    for (int y = 0; y < m_Height; y++)
    //    {
    //        for (int x = 0; x < m_Width; x++)
    //        {
    //            int i = 0;

    //            this[x, y][i++] = this[x, y + 1];
    //            this[x, y][i++] = this[x + 1, y];
    //            this[x, y][i++] = this[x, y - 1];
    //            this[x, y][i++] = this[x - 1, y];
    //        }
    //    }
    //}

    //public PathNode[] GetNeighbours(int x, int y)
    //{
    //    return new PathNode[8] { this[x,     y + 1],
    //                             this[x + 1, y + 1],
    //                             this[x + 1, y    ],
    //                             this[x + 1, y - 1],
    //                             this[x    , y - 1],
    //                             this[x - 1, y - 1],
    //                             this[x - 1, y    ],
    //                             this[x - 1, y + 1]};
    //}

    public PathNode[] GetNeighbours(int x, int y)
    {
        return new PathNode[4] { this[x,     y + 1],
                                 this[x + 1, y    ],
                                 this[x    , y - 1],
                                 this[x - 1, y    ] };
    }

    public PathNode[] GetNeighbours(PathNode node)
    {
        int index = Array.IndexOf(m_PathNodes, node);
        int x = index / m_Width;
        int y = index % m_Height;
        return GetNeighbours(x, y);
    }

    public void OnDrawGizmosSelected()
    {
        if(Application.isPlaying && enabled)
        {
            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    PathNode[] neighbours = GetNeighbours(x, y);

                    for (int i = 0; i < 4; i++)
                    {
                        PathNode node = neighbours[i];

                        if (node != null)
                        {
                            Gizmos.DrawRay(this[x, y].Position, (node.Position - this[x, y].Position).normalized * 0.5f);
                        }
                    }
                }
            }

            Gizmos.color = Color.red;

            if (m_Accessible != null)
            {
                for(int i = 0; i < m_Accessible.Length; i++)
                {
                    if(m_Accessible[i] != null)
                    {
                        Gizmos.DrawSphere(m_Accessible[i].Position, 0.2f);
                    }
                }
            }
        }
    }

    public List<PathNode> GetPath(int xFrom, int yFrom, int xTo, int yTo)
    {
        PathNode fromNode = this[xFrom, yFrom];
        PathNode toNode = this[xTo, yTo];

        Heap<PathNode> openSet = new Heap<PathNode>(m_Width * m_Height);
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        openSet.Add(fromNode);

        List<PathNode> path = new List<PathNode>();

        while (openSet.Count > 0)
        {
            PathNode currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            if (currentNode == toNode)
            {
                return RetracePath(fromNode, toNode);
            }

            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (neighbour == null || !neighbour.IsTraversible || closedSet.Contains(neighbour))
                {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, toNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        //openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        return null;
    }

    List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        return path;
    }

    float GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int aIndex = Array.IndexOf(m_PathNodes, nodeA);
        int aX = aIndex / m_Width;
        int aY = aIndex % m_Height;

        int bIndex = Array.IndexOf(m_PathNodes, nodeB);
        int bX = bIndex / m_Width;
        int bY = bIndex % m_Height;

        //int aX = Mathf.RoundToInt(nodeA.Position.z);
        //int aY = Mathf.RoundToInt(nodeA.Position.x);
        //int bX = Mathf.RoundToInt(nodeB.Position.z);
        //int bY = Mathf.RoundToInt(nodeB.Position.x);

        int dstX = Mathf.Abs(aX - bX);
        int dstY = Mathf.Abs(aY - bY);

        if (dstX >= dstY)
        {
            return 1.4f * dstY + (dstX - dstY);
        }
        return 1.4f * dstX + (dstY - dstX);
    }
}