using System.Collections.Generic;
using UnityEngine;
using System;

namespace Navigation
{
    public class Map : MonoBehaviour
    {
        private Node[] m_Nodes;
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

        public Node this[int x, int y]
        {
            get
            {
                //return m_NavigationNodes[(x * m_Height) + y];
                return (x >= 0 && x < m_Width && y >= 0 && y < m_Height) ? m_Nodes[(x * m_Height) + y] : null;
            }
            set
            {
                m_Nodes[(x * m_Height) + y] = value;
            }
        }

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            m_Nodes = new Node[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    this[x, y] = new Node()
                    {
                        Position = HexSpawner.HexPosFromGrid(x, y),
                        IsTraversible = true
                    };
                }
            }
        }

        //public Vector3 GetNodePosition(int x, int y)
        //{
        //    return new Vector3(x, 0, y);
        //}

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

        public Node[] GetConnected(int x, int y)
        {
            if((y & 1) == 0)
            {
                return new Node[6]
                {
                    this[x + 1, y    ],
                    this[x,     y + 1],
                    this[x - 1, y    ],
                    this[x,     y - 1],
                    this[x - 1, y + 1],
                    this[x - 1, y - 1]
                };
            }
            else
            {
                return new Node[6] 
                {
                    this[x + 1, y    ],
                    this[x    , y + 1],
                    this[x - 1, y    ],
                    this[x    , y - 1],
                    this[x + 1, y + 1],
                    this[x + 1, y - 1]
                };
            }
        }

        public Node[] GetConnected(Node node)
        {
            Vector2Int index = IndexOf(node);
            return GetConnected(index.x, index.y);
        }

        public Vector2Int IndexOf(Node node)
        {
            int index = Array.IndexOf(m_Nodes, node);
            //Vector2Int indices = new Vector2Int(index & m_Width, index / m_Width);
            Vector2Int indices = new Vector2Int(index / m_Height, index % m_Height);
            return indices;
        }

        public void OnDrawGizmos()
        {
            if (Application.isPlaying && enabled)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    for (int x = 0; x < m_Width; x++)
                    {
                        Node node = this[x, y];

                        if (!node.IsTraversible)
                        {
                            continue;
                        }

                        Gizmos.DrawSphere(node.Position, 0.2f);

                        Node[] neighbours = GetConnected(x, y);

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

        public List<Node> GetPath(int xFrom, int yFrom, int xTo, int yTo)
        {
            Node fromNode = this[xFrom, yFrom];
            Node toNode = this[xTo, yTo];

            return GetPath(fromNode, toNode);
        }

        public List<Node> GetPath(Node fromNode, Node toNode)
        {
            Heap<Node> openSet = new Heap<Node>(m_Width * m_Height);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(fromNode);

            List<Node> path = new List<Node>();

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    return RetracePath(fromNode, toNode);
                }

                foreach (Node connected in GetConnected(currentNode))
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

        List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }

        float GetDistance(Node from, Node to)
        {
            Vector2Int fromIndex = IndexOf(from);
            Vector2Int toIndex =   IndexOf(to);

            int dstX = Mathf.Abs(fromIndex.x - toIndex.x);
            int dstY = Mathf.Abs(fromIndex.y - toIndex.y);

            if (dstX >= dstY)
            {
                return 1.4f * dstY + (dstX - dstY);
            }
            return 1.4f * dstX + (dstY - dstX);
        }

        public Node GetRandom()
        {
            Node node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];

            while (!node.IsTraversible)
            {
                node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];
            }

            return node;
        }
    }
}