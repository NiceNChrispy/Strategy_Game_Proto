using System.Collections.Generic;
using UnityEngine;
using System;

namespace Navigation
{
    public class NavGraph : MonoBehaviour
    {
        private NavNode[] m_NavigationNodes;
        [SerializeField] int m_Width, m_Height;

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

        public NavNode this[int x, int y]
        {
            get
            {
                return (x >= 0 && x < m_Width && y >= 0 && y < m_Height) ? m_NavigationNodes[(x * m_Height) + y] : null;
            }

            set
            {
                m_NavigationNodes[(x * m_Height) + y] = value;
            }
        }

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            m_NavigationNodes = new NavNode[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    this[x, y] = new NavNode()
                    {
                        Position = GetNodePosition(x, y),
                        IsTraversible = true
                    };
                }
            }
        }

        //public Vector3 GetNodePosition(int x, int y)
        //{
        //    return new Vector3(x, 0, y);
        //}

        public Vector3 GetNodePosition(int row, int col)
        {
            float x = (col * 0.866f) + (0.433f * (row & 1));
            float z = row * 0.75f;

            return new Vector3(x, 0, z);
        }

        //public NavNode GetNodeAt(Vector3 position)
        //{
        //    int x = Mathf.RoundToInt(position.z + (position.x - (Mathf.RoundToInt(position.x) & 1)) / 2.0f);
        //    int y = Mathf.RoundToInt(position.x);

        //    print(x + " " + y);

        //    return this[x, y];
        //}

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

        //public NavNode[] GetConnected(int x, int y)
        //{
        //    return new NavNode[8] {  this[x,     y + 1],
        //                             this[x + 1, y + 1],
        //                             this[x + 1, y    ],
        //                             this[x + 1, y - 1],
        //                             this[x    , y - 1],
        //                             this[x - 1, y - 1],
        //                             this[x - 1, y    ],
        //                             this[x - 1, y + 1]};
        //}

        //public NavNode[] GetConnected(int x, int y)
        //{
        //    return new NavNode[4] { this[x,     y + 1],
        //                        this[x + 1, y    ],
        //                        this[x    , y - 1],
        //                        this[x - 1, y    ] };
        //}

        public NavNode[] GetConnected(int x, int y)
        {
            if(x % 2 == 0)
            {
                return new NavNode[6] { this[x + 1, y    ],
                                        this[x - 1, y - 1], // - -
                                        this[x    , y - 1],
                                        this[x - 1, y    ],
                                        this[x + 1, y - 1], // + -
                                        this[x    , y + 1] 
                };
            }
            else
            {
                return new NavNode[6] { this[x + 1, y    ],
                                        this[x + 1, y + 1], // + +
                                        this[x    , y - 1],
                                        this[x - 1, y    ],
                                        this[x - 1, y + 1], // - +
                                        this[x    , y + 1]};
            }
        }

        public NavNode[] GetConnected(NavNode node)
        {
            int index = Array.IndexOf(m_NavigationNodes, node);
            int x = index / m_Width;
            int y = index % m_Height;
            return GetConnected(x, y);
        }

        public void OnDrawGizmos()
        {
            if (Application.isPlaying && enabled)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    for (int x = 0; x < m_Width; x++)
                    {
                        NavNode node = this[x, y];

                        if (!node.IsTraversible)
                        {
                            continue;
                        }

                        Gizmos.DrawSphere(node.Position, 0.2f);

                        NavNode[] neighbours = GetConnected(x, y);

                        for (int i = 0; i < neighbours.Length; i++)
                        {
                            if (neighbours[i] != null && neighbours[i].IsTraversible)
                            {
                                Gizmos.DrawRay(node.Position, ((neighbours[i].Position - node.Position)) * 0.5f);
                            }
                        }
                    }
                }
            }
        }

        public List<NavNode> GetPath(int xFrom, int yFrom, int xTo, int yTo)
        {
            NavNode fromNode = this[xFrom, yFrom];
            NavNode toNode = this[xTo, yTo];

            return GetPath(fromNode, toNode);
        }

        public List<NavNode> GetPath(NavNode fromNode, NavNode toNode)
        {
            Heap<NavNode> openSet = new Heap<NavNode>(m_Width * m_Height);
            HashSet<NavNode> closedSet = new HashSet<NavNode>();

            openSet.Add(fromNode);

            List<NavNode> path = new List<NavNode>();

            while (openSet.Count > 0)
            {
                NavNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    return RetracePath(fromNode, toNode);
                }

                foreach (NavNode connected in GetConnected(currentNode))
                {
                    if (connected == null || !connected.IsTraversible || closedSet.Contains(connected))
                    {
                        continue;
                    }

                    float movementCost = currentNode.GCost + GetDistance(currentNode, connected);

                    if (movementCost < connected.GCost || !openSet.Contains(connected))
                    {
                        connected.GCost = movementCost;
                        connected.HCost = GetDistance(connected, toNode);
                        connected.Parent = currentNode;

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
            return null;
        }

        List<NavNode> RetracePath(NavNode startNode, NavNode endNode)
        {
            List<NavNode> path = new List<NavNode>();
            NavNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Add(startNode);
            path.Reverse();

            return path;
        }

        float GetDistance(NavNode from, NavNode to)
        {
            int fromIndex = Array.IndexOf(m_NavigationNodes, from);
            int fromX = fromIndex / m_Width;
            int fromY = fromIndex % m_Height;

            int toIndex = Array.IndexOf(m_NavigationNodes, to);
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

        public NavNode GetRandom()
        {
            NavNode node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];

            while (!node.IsTraversible)
            {
                node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];
            }

            return node;
        }
    }
}