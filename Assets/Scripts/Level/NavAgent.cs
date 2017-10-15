using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    [SerializeField] private NavGrid m_Grid;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_TurnSpeed;

    NavNode m_ActiveNode;

    public event Action OnPathUpdated = delegate { };
    public event Action OnPathingStarted = delegate { };
    public event Action OnPathingFinished = delegate { };

    private void Start()
    {
        transform.parent = m_Grid.transform;

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.z);

        m_ActiveNode = m_Grid[x, y];

        if(m_ActiveNode == null)
        {
            Debug.LogError(string.Format("Node {0},{1} is already occupied or is not traversible", x, y));
            gameObject.SetActive(false);
        }
        else
        {
            transform.localPosition = new Vector3(x, m_Grid.transform.position.y, y);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PathTo(m_Grid.GetRandom()));
        }
    }

    IEnumerator PathTo(NavNode targetNode)
    {
        List<NavNode> path = new List<NavNode>();
        path = m_Grid.GetPath(m_ActiveNode, targetNode);
        OnPathUpdated.Invoke();
        OnPathingStarted.Invoke();

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 vector = path[i].Position - m_ActiveNode.Position;
            Vector3 direction = vector.normalized;

            if (m_ActiveNode != path[i] && transform.forward != direction)
            {
                yield return StartCoroutine(Turn(direction));
            }

            yield return StartCoroutine(Move(path[i].Position));

            m_ActiveNode.IsTraversible = true;
            m_ActiveNode = path[i];
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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    if (path != null && path.Count > 1)
    //    {
    //        for (int i = 0; i < path.Count - 1; i++)
    //        {
    //            Gizmos.DrawLine(path[i].Position + Vector3.up, path[i + 1].Position + Vector3.up);
    //        }
    //    }
    //}
}