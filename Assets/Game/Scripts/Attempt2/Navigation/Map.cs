using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototype
{
    namespace Navigation
    {
        [ExecuteInEditMode]
        public class Map : MonoBehaviour
        {
            [SerializeField] private int m_RingCount;
            [SerializeField] private Node m_NodePrefab;

            [SerializeField] private Pattern m_Pattern;

            private Dictionary<Vector2Int, Node> m_NodeDict;

            private Vector3Int[] Directions =
            {
                new Vector3Int( 1, -1,  0),
                new Vector3Int( 1,  0, -1),
                new Vector3Int( 0,  1, -1),
                new Vector3Int(-1,  1,  0),
                new Vector3Int(-1,  0,  1),
                new Vector3Int( 0, -1,  1),
            };

            [NaughtyAttributes.Button("Create Grid")]
            private void CreateMap()
            {
                if (m_NodeDict != null && m_NodeDict.Count > 0)
                {
                    ClearPoints();
                }

                m_NodeDict = new Dictionary<Vector2Int, Node>();

                if (m_NodePrefab == null)
                {
                    Debug.LogError("Node object prefab is NULL!");
                    return;
                }

                SpawnPoints();
                ConnectPoints();
            }

            public Node GetNodeAt(Vector2Int coords)
            {
                Node node;
                m_NodeDict.TryGetValue(coords, out node);

                return node;
            }

            private void Awake()
            {
                ClearPoints();
                if (m_NodeDict == null || m_NodeDict.Count == 0)
                {
                    CreateMap();
                }
            }

            [NaughtyAttributes.Button("Clear Grid")]
            private void ClearPoints()
            {
                if (m_NodeDict != null)
                {
                    foreach (KeyValuePair<Vector2Int, Node> pair in m_NodeDict)
                    {
                        if (pair.Value != null)
                        {
                            DestroyImmediate(pair.Value.gameObject);
                        }
                    }
                    m_NodeDict.Clear();
                }

                foreach(Node node in FindObjectsOfType<Node>())
                {
                    DestroyImmediate(node.gameObject);
                }
            }

            private void SpawnPoints()
            {
                foreach (Vector2Int coord in m_Pattern.GetPositions())
                {
                    Node node = Instantiate(m_NodePrefab);
                    node.name = coord.ToString();
                    node.transform.position = transform.TransformPoint(HexUtil.WorldPosFromHexPos(coord));
                    node.transform.parent = transform;
                    node.WorldPosition = transform.TransformPoint(HexUtil.WorldPosFromHexPos(coord));
                    node.HexPosition = coord;

                    m_NodeDict.Add(coord, node);
                }
            }

            private void ConnectPoints()
            {
                foreach (KeyValuePair<Vector2Int, Node> pair in m_NodeDict)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Node node;
                        Vector2Int connectionPosition = HexUtil.CubeToHex(HexUtil.HexToCube(pair.Key) + Directions[i]);
                        if (m_NodeDict.TryGetValue(connectionPosition, out node))
                        {
                            pair.Value.AddConnected(node);
                        }
                    }
                }
            }

            public TValue RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
            {
                List<TValue> values = Enumerable.ToList(dict.Values);
                int size = dict.Count;

                return (values[UnityEngine.Random.Range(0, values.Count)]);
                
            }

            public Node GetRandomNode()
            {
                return RandomValues(m_NodeDict);
            }

            public List<Node> TilesInRange(Vector2Int position, int range)
            {
                List<Node> nodesInRange = new List<Node>();

                foreach (Vector2Int coord in HexUtil.CoordsInRange(position, range))
                {
                    Node node;

                    if (m_NodeDict.TryGetValue(coord, out node))
                    {
                        nodesInRange.Add(node);
                    }
                }

                return nodesInRange;
            }

            //public List<int> ReacableNodes(Vector2Int position, int range)
            //{
            //    List<NodeBehaviour> reacableNodes = new List<NodeBehaviour>();
            //    NodeBehaviour node;
            //
            //    if (m_NodeDict.TryGetValue(position, out node))
            //    {
            //        node.GetConnectedGraph(node, range, ref reacableNodes);
            //    }
            //    return reacableNodes;
            //}

            public List<Node> GetPath(Node fromNode, Node toNode)
            {
                Heap<Node> openSet = new Heap<Node>(m_NodeDict.Count);
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(fromNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();

                    closedSet.Add(currentNode);

                    if (currentNode == toNode)
                    {
                        return RetracePath(fromNode, toNode);
                    }

                    foreach (Node connectedNode in currentNode.ConnectedNodes)
                    {
                        if (connectedNode == null || !connectedNode.IsTraversible || closedSet.Contains(connectedNode))
                        {
                            continue;
                        }

                        float movementCost = currentNode.AStarData.GCost + GetDistance(currentNode, connectedNode);

                        if (movementCost < connectedNode.AStarData.GCost || !openSet.Contains(connectedNode))
                        {
                            connectedNode.AStarData.GCost = movementCost;
                            connectedNode.AStarData.HCost = GetDistance(connectedNode, toNode);
                            connectedNode.AStarData.Parent = currentNode;
                            if (!openSet.Contains(connectedNode))
                            {
                                openSet.Add(connectedNode);
                            }
                            else
                            {
                                openSet.UpdateItem(connectedNode);
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
                    currentNode = currentNode.AStarData.Parent;
                }

                path.Add(startNode);
                path.Reverse();

               return path;
            }

            float GetDistance(Node from, Node to)
            {
                return Vector3.Distance(from.transform.position, to.transform.position);
            }
        }
    }
}