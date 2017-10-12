using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    [SerializeField] private NavGrid m_Grid;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_TurnSpeed;

    [SerializeField] private int x, y;

    List<NavNode> path;

    NavNode m_ActiveNode;

    bool isMoving;

    private void Start()
    {
        path = new List<NavNode>();
        m_ActiveNode = m_Grid[x, y];
        transform.parent = m_Grid.transform;

        if(m_ActiveNode == null)
        {
            Debug.LogError(string.Format("Node {0},{1} is already occupied or is not traversible", x, y));
            gameObject.SetActive(false);
        }
        else
        {
            transform.localPosition = new Vector3(x, 1, y);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            UpdatePath(m_Grid.GetRandom());
            StartCoroutine(Move(path));
        }
    }

    IEnumerator Move(List<NavNode> path)
    {
        isMoving = true;

        for (int i = 0; i < path.Count; i++)
        {
            float t = 0;
            Vector3 vector = path[i].Position - m_ActiveNode.Position;
            Vector3 direction = vector.normalized;
            float distance = vector.magnitude;

            if (m_ActiveNode != path[i] && transform.forward != direction)
            {
                Quaternion from = transform.localRotation;
                Quaternion to = Quaternion.LookRotation(direction, Vector3.up);

                while (t < 1.0f)
                {
                    t += Time.deltaTime * m_TurnSpeed;
                    transform.localRotation = Quaternion.Lerp(from, to, t);
                    yield return new WaitForEndOfFrame();
                }
            }

            t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (m_MoveSpeed / distance);
                transform.localPosition = Vector3.Lerp(m_ActiveNode.Position, path[i].Position, t);
                yield return new WaitForEndOfFrame();
            }
            m_ActiveNode.IsTraversible = true;
            m_ActiveNode = path[i];
            m_ActiveNode.IsTraversible = false;
        }
        isMoving = false;
    }

    void UpdatePath(NavNode targetNode)
    {
        path.Clear();
        path = m_Grid.GetPath(m_ActiveNode, targetNode);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (path != null && path.Count > 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i].Position + Vector3.up, path[i + 1].Position + Vector3.up);
            }
        }
    }
}