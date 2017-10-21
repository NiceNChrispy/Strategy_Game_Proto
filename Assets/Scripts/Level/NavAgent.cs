using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class NavAgent : MonoBehaviour
    {
        [SerializeField] private NavGraph m_Graph;
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_TurnSpeed;

        NavNode m_ActiveNode;
        List<NavNode> m_ActivePath;

        public event Action OnStepComplete = delegate { };
        public event Action OnPathUpdated = delegate { };
        public event Action OnPathingStarted = delegate { };
        public event Action OnPathingFinished = delegate { };

        public bool HasPath
        {
            get { return m_ActivePath != null && m_ActivePath.Count > 1; }
        }

        public NavNode ActiveNode
        {
            get
            {
                return m_ActiveNode;
            }
        }

        private void Start()
        {
            transform.parent = m_Graph.transform;

            m_ActiveNode = m_Graph[1, 1];
            m_ActiveNode.IsTraversible = false;

            if (m_ActiveNode == null)
            {
                //Debug.LogError(string.Format("Node {0},{1} is already occupied or is not traversible", x, y));
                gameObject.SetActive(false);
            }
            else
            {
                transform.localPosition = m_ActiveNode.Position;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(PathTo(m_Graph.GetRandom()));
            }
        }

        public IEnumerator PathTo(NavNode targetNode)
        {
            if(targetNode == null)
            {
                Debug.LogError("Target node is null");
                yield break;
            }
            m_ActivePath = m_Graph.GetPath(m_ActiveNode, targetNode);

            if (!HasPath)
            {
                yield break;
            }

            OnPathUpdated.Invoke();
            OnPathingStarted.Invoke();

            for (int i = 0; i < m_ActivePath.Count; i++)
            {
                Vector3 vector = m_ActivePath[i].Position - m_ActiveNode.Position;
                Vector3 direction = vector.normalized;

                if (m_ActiveNode != m_ActivePath[i] && transform.forward != direction)
                {
                    yield return StartCoroutine(Turn(direction));
                }

                yield return StartCoroutine(Move(m_ActivePath[i].Position));

                m_ActiveNode.IsTraversible = true;
                m_ActiveNode = m_ActivePath[i];
                OnStepComplete.Invoke();
                m_ActiveNode.IsTraversible = false;
            }
            OnPathingFinished.Invoke();
        }

        IEnumerator Move(Vector3 position)
        {
            float distance = (position - m_ActiveNode.Position).magnitude;
            float t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (m_MoveSpeed / distance);
                transform.localPosition = Vector3.Lerp(m_ActiveNode.Position, position, t);
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            if (HasPath)
            {
                for (int i = 0; i < m_ActivePath.Count - 1; i++)
                {
                    Gizmos.DrawLine(m_ActivePath[i].Position, m_ActivePath[i + 1].Position);
                }
                for (int i = 0; i < m_ActivePath.Count; i++)
                {
                    Gizmos.DrawSphere(m_ActivePath[i].Position, 0.2f);
                }
            }
        }
    }
}