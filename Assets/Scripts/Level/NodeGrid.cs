using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeGrid : MonoBehaviour
{
    private PathNode[] m_PathNodes;
    [SerializeField] int m_Width, m_Height;

    private Dictionary<PathNode, Agent> m_OccupiedNodes;

    public int Width
    {
        get
        {
            return m_Width;
        }
    }
    public int Height
    {
        get
        {
            return m_Height;
        }
    }

    public PathNode this[int x, int y]
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

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        m_PathNodes = new PathNode[m_Width * m_Height];

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                this[x, y] = new PathNode()
                {
                    Position = new Vector3(x, 1, y),
                    IsTraversible = true
                };
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

    public void OnDrawGizmos()
    {
        if(Application.isPlaying && enabled)
        {
            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    if(!this[x,y].IsTraversible)
                    {
                        continue;
                    }
                    PathNode[] neighbours = GetNeighbours(x, y);

                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        PathNode node = neighbours[i];

                        if (node != null && node.IsTraversible)
                        {
                            Gizmos.DrawRay(this[x, y].Position, ((node.Position - this[x, y].Position)) * 0.5f);
                        }
                    }
                }
            }

            //Gizmos.color = Color.red;

            //if (m_Accessible != null)
            //{
            //    for(int i = 0; i < m_Accessible.Length; i++)
            //    {
            //        if(m_Accessible[i] != null)
            //        {
            //            Gizmos.DrawSphere((Vector2)m_Accessible[i].Position, 0.2f);

            //        }
            //    }
            //    for (int i = 0; i < m_Accessible.Length - 1; i++)
            //    {
            //        if (m_Accessible[i] != null)
            //        {
            //            Gizmos.DrawLine((Vector2)m_Accessible[i].Position, (Vector2)m_Accessible[i + 1].Position);
            //        }
            //    }
            //}
        }
    }

    public List<PathNode> GetPath(int xFrom, int yFrom, int xTo, int yTo)
    {
        PathNode fromNode = this[xFrom, yFrom];
        PathNode toNode = this[xTo, yTo];

        return GetPath(fromNode, toNode);
    }

    public List<PathNode> GetPath(PathNode fromNode, PathNode toNode)
    {
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
                        openSet.UpdateItem(neighbour);
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
        path.Add(startNode);
        path.Reverse();

        return path;
    }

    float GetDistance(PathNode from, PathNode to)
    {
        int fromIndex = Array.IndexOf(m_PathNodes, from);
        int fromX = fromIndex / m_Width;
        int fromY = fromIndex % m_Height;

        int toIndex = Array.IndexOf(m_PathNodes, to);
        int toX = toIndex / m_Width;
        int toY = toIndex % m_Height;

        int dstX = Mathf.Abs(fromX - toX);
        int dstY = Mathf.Abs(fromY - toY);

        if (dstX >= dstY)
        {
            return 1.4f * dstY + (dstX - dstY);
        }
        return 1.4f * dstX + (dstY - dstX);
    }

    //public bool AddAgent(Agent agent, int x, int y)
    //{
    //    PathNode node = this[x, y];
    //    if (m_OccupiedNodes.ContainsKey(node))
    //    {
    //        return false;
    //    }
    //    m_OccupiedNodes.Add(node, agent);
    //    return true;
    //}

    public PathNode Occupy(int x, int y)
    {
        PathNode node = this[x, y];

        if(node.IsTraversible)
        {
            node.IsTraversible = false;
            return node;
        }
        return null;
    }
}