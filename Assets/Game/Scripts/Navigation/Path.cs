using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Path
    {
        private List<Node> m_Nodes;

        public Node Start { get { return m_Nodes[0]; } }
        public Node End { get { return m_Nodes[m_Nodes.Count - 1]; } }

        public Path(List<Node> path)
        {
            if(path.Count > 1)
            {
                m_Nodes = path;
                Debug.LogError("Path must contain at least two nodes");
            }
        }

        public float Length()
        {
            float length = 0;

            for (int i = 0; i < m_Nodes.Count - 1; i++)
            {
                length += Vector3.Distance(m_Nodes[i].Position, m_Nodes[i + 1].Position);
            }

            return length;
        }
    }
}