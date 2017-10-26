using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Path<T> : IList<AStarNode>, IEnumerable
    {
        private List<AStarNode> m_Nodes;

        public AStarNode Start { get { return m_Nodes[0]; } }
        public AStarNode End { get { return m_Nodes[m_Nodes.Count - 1]; } }

        public int Count { get { return ((IList<AStarNode>)m_Nodes).Count; } }

        public bool IsReadOnly { get { return ((IList<AStarNode>)m_Nodes).IsReadOnly; } }

        public AStarNode this[int index] { get { return ((IList<AStarNode>)m_Nodes)[index]; } set { ((IList<AStarNode>)m_Nodes)[index] = value; } }

        public float m_Distance;

        public Path()
        {
            m_Nodes = new List<AStarNode>();
        }

        public Path(List<AStarNode> path)
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
                        distance += Vector3.Distance(m_Nodes[i].Data.Position, m_Nodes[i + 1].Data.Position);
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

        public int IndexOf(AStarNode item)
        {
            return ((IList<AStarNode>)m_Nodes).IndexOf(item);
        }

        public void Insert(int index, AStarNode item)
        {
            ((IList<AStarNode>)m_Nodes).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<AStarNode>)m_Nodes).RemoveAt(index);
        }

        public void Add(AStarNode item)
        {
            ((IList<AStarNode>)m_Nodes).Add(item);
        }

        public void Clear()
        {
            ((IList<AStarNode>)m_Nodes).Clear();
        }

        public bool Contains(AStarNode item)
        {
            return ((IList<AStarNode>)m_Nodes).Contains(item);
        }

        public void CopyTo(AStarNode[] array, int arrayIndex)
        {
            ((IList<AStarNode>)m_Nodes).CopyTo(array, arrayIndex);
        }

        public bool Remove(AStarNode item)
        {
            return ((IList<AStarNode>)m_Nodes).Remove(item);
        }

        IEnumerator<AStarNode> IEnumerable<AStarNode>.GetEnumerator()
        {
            return ((IList<AStarNode>)m_Nodes).GetEnumerator();
        }

        public void Reverse()
        {
            m_Nodes.Reverse();
        }
    }
}