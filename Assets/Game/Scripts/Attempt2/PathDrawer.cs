using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    namespace Navigation
    {
        public class PathDrawer : MonoBehaviour
        {
            [SerializeField] private Map m_Map;
            [SerializeField] List<Node> m_Path;

            [SerializeField] Node from;
            [SerializeField] Node to;

            [SerializeField] private LineRenderer m_PathLine;

            void Update()
            {
                Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
                Ray ray = Camera.main.ScreenPointToRay(cursorPos);
                Debug.DrawLine(ray.origin, ray.direction * 100);

                if (Input.GetMouseButtonDown(0))
                {
                    Node node = Utility.ObjectOfTypeUnderCursor<Node>();
                    if (node)
                    {
                        from = node;
                        if (to)
                        {
                            m_Path = m_Map.GetPath(from, to);
                            SetPathPoints();
                        }
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Node node = Utility.ObjectOfTypeUnderCursor<Node>();
                    if (node && from)
                    {
                        to = node;
                        if (from)
                        {
                            m_Path = m_Map.GetPath(from, to);
                            SetPathPoints();
                        }
                    }
                }
            }

            private void OnDrawGizmos()
            {
                if (m_Path != null && m_Path.Count > 1)
                {
                    for (int i = 0; i < m_Path.Count - 1; i++)
                    {
                        Gizmos.DrawLine(m_Path[i].transform.position + Vector3.up, m_Path[i + 1].transform.position + Vector3.up);
                    }
                }

                if (from)
                {
                    Gizmos.DrawSphere(from.transform.position, 0.2f);
                }

                if (to)
                {
                    Gizmos.DrawSphere(to.transform.position, 0.2f);
                }
            }

            void SetPathPoints()
            {
                m_PathLine.positionCount = m_Path.Count;

                for (int i = 0; i < m_Path.Count; i++)
                {
                    m_PathLine.SetPosition(i, m_Path[i].WorldPosition + Vector3.up * 0.01f);
                }
            }
        }
    }
}