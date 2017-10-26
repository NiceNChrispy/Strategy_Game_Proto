using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

namespace Navigation
{
    public class Graph<T> : IList<Node<T>> where T: IComparable<T>
    {
        List<Node<T>> m_Nodes;

        Node<T> IList<Node<T>>.this[int index]
        {
            get
            {
                return ((IList<Node<T>>)m_Nodes)[index];
            }

            set
            {
                ((IList<Node<T>>)m_Nodes)[index] = value;
            }
        }

        public int Count { get { return m_Nodes.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(Node<T> node)
        {
            ((IList<Node<T>>)m_Nodes).Add(node);
        }

        public void Clear()
        {
            ((IList<Node<T>>)m_Nodes).Clear();
        }

        public bool Contains(Node<T> node)
        {
            return ((IList<Node<T>>)m_Nodes).Contains(node);
        }

        public void CopyTo(Node<T>[] array, int arrayIndex)
        {
            ((IList<Node<T>>)m_Nodes).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return ((IList<Node<T>>)m_Nodes).GetEnumerator();
        }

        public int IndexOf(Node<T> node)
        {
            return ((IList<Node<T>>)m_Nodes).IndexOf(node);
        }

        public void Insert(int index, Node<T> node)
        {
            ((IList<Node<T>>)m_Nodes).Insert(index, node);
        }

        public bool Remove(Node<T> node)
        {
            return ((IList<Node<T>>)m_Nodes).Remove(node);
        }

        public void RemoveAt(int index)
        {
            ((IList<Node<T>>)m_Nodes).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Node<T>>)m_Nodes).GetEnumerator();
        }
    }

    public class Map : MonoBehaviour
    {
        private Dictionary<Vector2Int, AStarNode> m_Nodes;
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

        public AStarNode this[int x, int y]
        {
            get
            {
                //return m_NavigationNodes[(x * m_Height) + y];
                return (x >= 0 && x < m_Width && y >= 0 && y < m_Height) ? m_Nodes[new Vector2Int(x, y)] : null;
            }
            set
            {
                m_Nodes[new Vector2Int(x,y)] = value;
            }
        }

        private void OnEnable()
        {
            Init();
            Connect();
        }

        public void Init()
        {
            m_Nodes = new Dictionary<Vector2Int, AStarNode>();

            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    m_Nodes.Add(new Vector2Int(x, y),
                                new AStarNode(new AStarData()
                                {
                                    Position = HexSpawner.HexPosFromGrid(x, y),
                                    IsTraversible = true
                                }));
                }
            }
        }

        public void Connect()
        {
            int connected = 0;
            foreach (KeyValuePair<Vector2Int, AStarNode> keyValue in m_Nodes)
            {
                int x = keyValue.Key.x;
                int y = keyValue.Key.y;

                AStarNode[] connections = new AStarNode[6];

                connections[0] = this[x + 1, y    ];
                connections[1] = this[x    , y + 1];
                connections[2] = this[x - 1, y    ];
                connections[3] = this[x    , y - 1];

                if ((y & 1) == 0)
                {
                    connections[4] = this[x - 1, y + 1];
                    connections[5] = this[x - 1, y - 1];
                }
                else
                { 
                    connections[4] = this[x + 1, y + 1];
                    connections[5] = this[x + 1, y - 1];
                }

                for (int i = 0; i < 6; i++)
                {
                    if(connections[i] != null)
                    {
                        keyValue.Value.AddConnection(connections[i]);
                        connected++;
                    }
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

        //public AStarNode[] GetConnected(int x, int y)
        //{
        //    if((y & 1) == 0)
        //    {
        //        return new AStarNode[6]
        //        {
        //            this[x + 1, y    ],
        //            this[x,     y + 1],
        //            this[x - 1, y    ],
        //            this[x,     y - 1],
        //            this[x - 1, y + 1],
        //            this[x - 1, y - 1]
        //        };
        //    }
        //    else
        //    {
        //        return new AStarNode[6] 
        //        {
        //            this[x + 1, y    ],
        //            this[x    , y + 1],
        //            this[x - 1, y    ],
        //            this[x    , y - 1],
        //            this[x + 1, y + 1],
        //            this[x + 1, y - 1]
        //        };
        //    }
        //}

        //public Node[] GetConnected(Node node)
        //{
        //    Vector2Int index = IndexOf(node);
        //    return GetConnected(index.x, index.y);
        //}

        //public Vector2Int IndexOf(AStarNode node)
        //{
        //    int index = Array.IndexOf(m_Nodes, node);
        //    //Vector2Int indices = new Vector2Int(index & m_Width, index / m_Width);
        //    Vector2Int indices = new Vector2Int(index / m_Height, index % m_Height);
        //    return indices;
        //}

        public void OnDrawGizmos()
        {
            if (Application.isPlaying && enabled)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    for (int x = 0; x < m_Width; x++)
                    {
                        AStarNode node = this[x, y];

                        if (!node.Data.IsTraversible)
                        {
                            Gizmos.color = Color.gray;
                            Gizmos.DrawSphere(node.Data.Position, 0.1f);
                            continue;
                        }

                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(node.Data.Position, 0.1f);

                        foreach (AStarNode connectedNode in node.Connections)
                        {
                            if (connectedNode.Data.IsTraversible)
                            {
                                Gizmos.DrawRay(node.Data.Position, ((connectedNode.Data.Position - node.Data.Position)) * 0.5f);
                            }
                        }
                    }
                }
            }
        }

        public List<AStarNode> GetPath(int xFrom, int yFrom, int xTo, int yTo)
        {
            AStarNode fromNode = this[xFrom, yFrom];
            AStarNode toNode = this[xTo, yTo];

            return GetPath(fromNode, toNode);
        }

        public List<AStarNode> GetPath(AStarNode fromNode, AStarNode toNode)
        {
            Heap<AStarNode> openSet = new Heap<AStarNode>(m_Width * m_Height);
            HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

            openSet.Add(fromNode);

            List<AStarNode> path = new List<AStarNode>();

            while (openSet.Count > 0)
            {
                AStarNode currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    return RetracePath(fromNode, toNode);
                }

                foreach (AStarNode connected in currentNode.Connections)
                {
                    if (connected == null || !connected.Data.IsTraversible || closedSet.Contains(connected))
                    {
                        continue;
                    }

                    float movementCost = currentNode.Data.GCost + GetDistance(currentNode, connected);

                    if (movementCost < connected.Data.GCost || !openSet.Contains(connected))
                    {
                        connected.Data.GCost = movementCost;
                        connected.Data.HCost = GetDistance(connected, toNode);
                        connected.Data.Parent = currentNode;

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

        List<AStarNode> RetracePath(AStarNode startNode, AStarNode endNode)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Data.Parent;
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }

        float GetDistance(AStarNode from, AStarNode to)
        {
            //Vector2Int fromIndex = m_Nodes.FirstOrDefault(x => x.Value == from).Key;
            //Vector2Int toIndex = m_Nodes.FirstOrDefault(x => x.Value == to).Key;
            return Vector3.Distance(from.Data.Position, to.Data.Position) * 10;
            //
            //int dstX = Mathf.Abs(fromIndex.x - toIndex.x);
            //int dstY = Mathf.Abs(fromIndex.y - toIndex.y);
            //
            //if (dstX >= dstY)
            //{
            //    return 1.4f * dstY + (dstX - dstY);
            //}
            //return 1.4f * dstX + (dstY - dstX);
        }

        public AStarNode GetRandom()
        {
            AStarNode node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];

            while (!node.Data.IsTraversible)
            {
                node = this[UnityEngine.Random.Range(0, m_Width), UnityEngine.Random.Range(0, m_Height)];
            }

            return node;
        }
    }
}