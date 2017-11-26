using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private Map m_Map;
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_TurnSpeed;

        public bool IsMoving { get; protected set; }

        AStarNode m_ActiveNode;
        AStarNode m_NextNode;

        List<AStarNode> m_Path;

        public event Action OnStepComplete = delegate { };
        public event Action OnPathUpdated = delegate { };
        public event Action OnPathingStarted = delegate { };
        public event Action OnPathingFinished = delegate { };

        public bool HasPath
        {
            get { return m_Path != null && m_Path.Count > 1; }
        }

        public AStarNode ActiveNode
        {
            get
            {
                return m_ActiveNode;
            }
        }
        public AStarNode NextNode
        {
            get
            {
                return m_NextNode;
            }
        }

        public float StepCompletionPercent
        {
            get
            {
                if (HasPath)
                {
                    return Mathf.InverseLerp(m_ActiveNode.Data.Position.sqrMagnitude, m_NextNode.Data.Position.sqrMagnitude, transform.position.sqrMagnitude);
                }
                return 0;
            }
        }

        public List<AStarNode> Path
        {
            get
            {
                return m_Path;
            }
        }

        private void Start()
        {
            transform.parent = m_Map.transform;

            m_ActiveNode = m_Map[3, 3];
            m_ActiveNode.Data.IsTraversible = false;

            if (m_ActiveNode == null)
            {
                //Debug.LogError(string.Format("Node {0},{1} is already occupied or is not traversible", x, y));
                gameObject.SetActive(false);
            }
            else
            {
                transform.localPosition = m_ActiveNode.Data.Position;
            }
        }

        [NaughtyAttributes.Button("Path To Random")]
        private void Debug_PathToRandom()
        {
            StartCoroutine(PathRoutine(m_Map.GetRandom(), delegate {}));
        }

        public void MoveTo(AStarNode targetNode, Action callback)
        {
            if(!IsMoving)
            {
                StartCoroutine(PathRoutine(targetNode, callback));
            }
        }

        private IEnumerator PathRoutine(AStarNode targetNode, Action callback)
        {
            if (targetNode == null)
            {
                Debug.LogError("Target node is null");
                yield break;
            }

            m_Path = m_Map.GetPath(m_ActiveNode, targetNode);

            if (!HasPath)
            {
                yield break;
            }


            IsMoving = true;

            OnPathUpdated.Invoke();
            OnPathingStarted.Invoke();

            for (int i = 0; i < m_Path.Count; i++)
            {
                m_NextNode = m_Path[i];
                Vector3 vector = m_NextNode.Data.Position - m_ActiveNode.Data.Position;
                Vector3 direction = vector.normalized;

                if (m_ActiveNode != m_NextNode && transform.forward != direction)
                {
                    yield return StartCoroutine(Turn(direction));
                }

                yield return StartCoroutine(Move(m_NextNode.Data.Position));

                m_ActiveNode.Data.IsTraversible = true;
                m_ActiveNode = m_NextNode;
                OnStepComplete.Invoke();
                m_ActiveNode.Data.IsTraversible = false;
            }
            IsMoving = false;
            OnPathingFinished.Invoke();
            callback.Invoke();

        }

        IEnumerator Move(Vector3 position)
        {
            float distance = (position - m_ActiveNode.Data.Position).magnitude;
            float t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (m_MoveSpeed / distance);
                transform.localPosition = Vector3.Lerp(m_ActiveNode.Data.Position, position, t);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator Turn(Vector3 direction)
        {
            float t = 0;

            Quaternion from = transform.localRotation;
            Quaternion to = Quaternion.LookRotation(direction, Vector3.up);

            while (t < 1.0f)
            {
                t += Time.deltaTime * m_TurnSpeed;
                transform.localRotation = Quaternion.Lerp(from, to, t);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;

            if (m_Path != null)
            {
                for (int i = 0; i < m_Path.Count - 1; i++)
                {
                    Gizmos.DrawLine(m_Path[i].Data.Position, m_Path[i + 1].Data.Position);
                }
                for (int i = 0; i < m_Path.Count; i++)
                {
                    Gizmos.DrawSphere(m_Path[i].Data.Position, 0.15f);
                }
            }
        }
    }
}