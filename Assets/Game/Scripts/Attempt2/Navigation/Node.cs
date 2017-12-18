using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    namespace Navigation
    {
        public class AStarData<T>
        {
            public float GCost;
            public float HCost;
            public float FCost { get { return GCost + HCost; } }

            public T Parent { get; internal set; }

            public int CompareTo(AStarData<T> other)
            {
                int compare = FCost.CompareTo(other.FCost);
                if (compare == 0)
                {
                    compare = HCost.CompareTo(other.HCost);
                }
                return -compare;
            }
        }

        public class Node : MonoBehaviour, IHeapItem<Node>
        {
            [SerializeField] private List<Node> m_ConnectedNodes;

            public Vector3 WorldPosition;
            public Vector2Int HexPosition;
            public bool IsTraversible;
            public AStarData<Node> AStarData;

            public List<Node> ConnectedNodes
            {
                get
                {
                    return m_ConnectedNodes;
                }
            }

            public int HeapIndex
            {
                get; set;
            }

            void Awake()
            {
                if (m_ConnectedNodes == null)
                {
                    m_ConnectedNodes = new List<Node>();
                }

                AStarData = new AStarData<Node>();
            }

            public void AddConnected(Node node)
            {
                m_ConnectedNodes.Add(node);
            }

            void OnDrawGizmos()
            {
                if (m_ConnectedNodes != null)
                {
                    foreach (Node node in ConnectedNodes)
                    {
                        Gizmos.DrawLine(transform.position, node.transform.position);
                    }
                }
            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = IsTraversible ? Color.white : Color.black;
                Gizmos.DrawSphere(transform.position, 0.1f);
            }

            public int CompareTo(Node other)
            {
                return AStarData.CompareTo(other.AStarData);
            }
        }

        //public class NodeData : IHeapItem<NodeData>
        //{
        //    public Vector3 WorldPosition;
        //    public Vector2Int HexPosition;
        //    public Node Node;
        //    public AStarData<NodeData> AStarData;
        //
        //    public NodeData(Vector3 worldPosition, Vector2Int hexPosition)
        //    {
        //        WorldPosition = worldPosition;
        //        HexPosition = hexPosition;
        //        AStarData = new AStarData<NodeData>();
        //        AStarData.IsTraversible = true;
        //    }
        //
        //    public int HeapIndex
        //    {
        //        get; set;
        //    }
        //
        //    public int CompareTo(NodeData other)
        //    {
        //        return AStarData.CompareTo(other.AStarData);
        //    }
        //}
    }
}