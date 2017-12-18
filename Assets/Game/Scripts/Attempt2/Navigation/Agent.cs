using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    namespace Navigation
    {
        public class Agent : MonoBehaviour
        {
            [SerializeField] private Vector2Int Coords;
            [SerializeField] private Node m_OccupiedNode;
            [SerializeField] private int m_MovementRange;

            List<Node> NodesInRange;
            [SerializeField] private List<Node> m_Path;

            [SerializeField] private float m_MoveSpeed;
            [SerializeField] private float m_TurnSpeed;
            [SerializeField] private Map m_Map;

            [SerializeField] private bool m_IsMoving;

            public bool IsMoving
            {
                get
                {
                    return m_IsMoving;
                }
            }

            public List<Node> Path
            {
                get
                {
                    return m_Path;
                }
            }

            public Node OccupiedNode
            {
                get
                {
                    return m_OccupiedNode;
                }
            }

            [NaughtyAttributes.Button("Init")]
            void Init()
            {
                Spawn(m_Map.GetNodeAt(Coords));
            }

            private void Start()
            {
                Spawn(m_Map.GetNodeAt(Coords));
            }

            public void Spawn(Node node)
            {
                m_OccupiedNode = node;
                m_OccupiedNode.IsTraversible = false;
                transform.position = node.transform.position;
            }

            public void Move()
            {
                if (!m_IsMoving && Path.Count > 0)
                {
                    StartCoroutine(PathfindRoutine());
                }
            }

            public void UpdatePathTo(Node node)
            {
                if (!m_IsMoving)
                {
                    UpdateTilesInRange();

                    //METHOD 1: only get path if destination node is accessible

                    //if (NodesInRange.Contains(node))
                    //{
                    //    m_Path = m_Map.GetPath(m_OccupiedNode, node);
                    //}

                    //METHOD 2: Cut path short to only accessible nodes
                    
                    m_Path = m_Map.GetPath(m_OccupiedNode, node);
                    
                    if (m_Path.Count > m_MovementRange + 1)
                    {
                        m_Path.RemoveRange(m_MovementRange + 1, m_Path.Count - m_MovementRange - 1);
                    }
                }
            }

            [NaughtyAttributes.Button("Move Random")]
            void MoveRandom()
            {
                UpdatePathTo(m_Map.GetRandomNode());
                Move();
            }

            IEnumerator PathfindRoutine()
            {
                m_IsMoving = true;
                int index = 0;
                Node targetNode = m_Path[m_Path.Count - 1];

                while (m_OccupiedNode != targetNode)
                {
                    transform.LookAt(m_Path[index].transform);
                    yield return MoveRoutine(m_Path[index++]);
                }
                m_IsMoving = false;
            }

            private IEnumerator MoveRoutine(Node targetNode)
            {
                float distance = (targetNode.WorldPosition - m_OccupiedNode.WorldPosition).magnitude;
                float t = 0;
                float step = (m_MoveSpeed / distance);

                while (t < 1.0f)
                {
                    t += Time.deltaTime * step;
                    transform.localPosition = Vector3.Lerp(m_OccupiedNode.WorldPosition, targetNode.WorldPosition, t);
                    yield return new WaitForEndOfFrame();
                }
                m_OccupiedNode.IsTraversible = true;
                m_OccupiedNode = targetNode;
                m_OccupiedNode.IsTraversible = false;
            }

            IEnumerator RotateRoutine(Node targetNode)
            {
                float t = 0;

                Quaternion from = transform.localRotation;
                Quaternion to = Quaternion.LookRotation((targetNode.WorldPosition - transform.localPosition).normalized, Vector3.up);

                while (t < 1.0f)
                {
                    t += Time.deltaTime * m_TurnSpeed;
                    transform.localRotation = Quaternion.Lerp(from, to, t);
                    yield return new WaitForEndOfFrame();
                }
            }

            [NaughtyAttributes.Button("UpdateTilesInRange")]
            public void UpdateTilesInRange()
            {
                NodesInRange = m_Map.TilesInRange(m_OccupiedNode.HexPosition, m_MovementRange);
            }

            public void ClearTilesInRange()
            {
                NodesInRange.Clear();
                NodesInRange = null;
            }

            void OnDrawGizmosSelected()
            {
                if (NodesInRange != null)
                {
                    Gizmos.color = Color.red;
                    foreach (Node node in NodesInRange)
                    {
                        Gizmos.DrawSphere(node.WorldPosition, 0.05f);
                    }
                }
                if (m_Path != null)
                {
                    Gizmos.color = Color.blue;
                    foreach (Node node in m_Path)
                    {
                        Gizmos.DrawSphere(node.WorldPosition, 0.1f);
                    }
                }
            }
        }
    }
}