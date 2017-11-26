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

        public event Action OnStepComplete = delegate { };
        public event Action OnPathingStarted = delegate { };
        public event Action OnPathingFinished = delegate { };

        public bool HasPath
        {
            get { return Path != null && Path.Count > 1; }
        }

        public bool HasDestination { get { return DestinationNode != null; } }

        public AStarNode ActiveNode      { get; private set; }
        public AStarNode NextNode        { get; private set; }
        public AStarNode DestinationNode { get; private set; }

        public List<AStarNode> Path      { get; private set; }

        public bool IsPathing { get; private set; }

        private void Start()
        {
            transform.parent = m_Map.transform;

            ActiveNode = m_Map.GetRandom();
            ActiveNode.Data.IsTraversible = false;

            if (ActiveNode == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                transform.localPosition = ActiveNode.Data.Position;
            }
        }

        [NaughtyAttributes.Button("Path To Random")]
        private void Debug_PathToRandom()
        {
            SetDestination(m_Map.GetRandom());
            BeginPathing();
        }

        public void SetDestination(AStarNode targetNode)
        {
            Path = m_Map.GetPath(ActiveNode, targetNode);

            /* If the target node is inaccessible */
            if(!HasPath)
            {
                ClearDestination();
                return;
            }

            DestinationNode = targetNode;
        }

        public void ClearDestination()
        {
            Path = null;
            DestinationNode = null;
        }

        public void BeginPathing()
        {
            if(!HasPath)
            {
                return;
            }
            StartCoroutine(PathRoutine());
        }

        private IEnumerator PathRoutine()
        {
            IsPathing = true;

            OnPathingStarted.Invoke();

            for (int i = 0; i < Path.Count; i++)
            {
                NextNode = Path[i];
                Vector3 vector = NextNode.Data.Position - ActiveNode.Data.Position;
                Vector3 direction = vector.normalized;

                if (ActiveNode != NextNode && transform.forward != direction)
                {
                    yield return StartCoroutine(Turn(direction));
                }

                yield return StartCoroutine(Move(NextNode.Data.Position));

                ActiveNode.Data.IsTraversible = true;
                ActiveNode = NextNode;
                OnStepComplete.Invoke();
                ActiveNode.Data.IsTraversible = false;
            }
            IsPathing = false;
            OnPathingFinished.Invoke();
        }

        IEnumerator Move(Vector3 position)
        {
            float distance = (position - ActiveNode.Data.Position).magnitude;
            float t = 0;
            float step = (m_MoveSpeed / distance);

            while (t < 1.0f)
            {
                t += Time.deltaTime * step;
                transform.localPosition = Vector3.Lerp(ActiveNode.Data.Position, position, t);
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

            if (Path != null)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    Gizmos.DrawLine(Path[i].Data.Position, Path[i + 1].Data.Position);
                }
                for (int i = 0; i < Path.Count; i++)
                {
                    Gizmos.DrawSphere(Path[i].Data.Position, 0.15f);
                }
            }
        }
    }
}