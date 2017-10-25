using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Path : IList<Node>, IEnumerable
    {
        private List<Node> m_Nodes;

        public Node Start { get { return m_Nodes[0]; } }
        public Node End { get { return m_Nodes[m_Nodes.Count - 1]; } }

        public int Count { get { return ((IList<Node>)m_Nodes).Count; } }

        public bool IsReadOnly { get { return ((IList<Node>)m_Nodes).IsReadOnly; } }

        public Node this[int index] { get { return ((IList<Node>)m_Nodes)[index]; } set { ((IList<Node>)m_Nodes)[index] = value; } }

        public float m_Distance;

        public Path()
        {
            m_Nodes = new List<Node>();
        }

        public Path(List<Node> path)
        {
            if(path.Count > 1)
            {
                m_Nodes = path;
                Debug.LogError("Path must contain at least two nodes");
            }
        }

        public float Distance
        {
            get
            {
                if (m_Distance == 0)
                {
                    float distance = 0;

                    for (int i = 0; i < m_Nodes.Count - 1; i++)
                    {
                        distance += Vector3.Distance(m_Nodes[i].Position, m_Nodes[i + 1].Position);
                    }

                    m_Distance = distance;
                }
                return m_Distance;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)m_Nodes).GetEnumerator();
        }

        public int IndexOf(Node item)
        {
            return ((IList<Node>)m_Nodes).IndexOf(item);
        }

        public void Insert(int index, Node item)
        {
            ((IList<Node>)m_Nodes).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Node>)m_Nodes).RemoveAt(index);
        }

        public void Add(Node item)
        {
            ((IList<Node>)m_Nodes).Add(item);
        }

        public void Clear()
        {
            ((IList<Node>)m_Nodes).Clear();
        }

        public bool Contains(Node item)
        {
            return ((IList<Node>)m_Nodes).Contains(item);
        }

        public void CopyTo(Node[] array, int arrayIndex)
        {
            ((IList<Node>)m_Nodes).CopyTo(array, arrayIndex);
        }

        public bool Remove(Node item)
        {
            return ((IList<Node>)m_Nodes).Remove(item);
        }

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return ((IList<Node>)m_Nodes).GetEnumerator();
        }

        public void Reverse()
        {
            m_Nodes.Reverse();
        }
    }
}