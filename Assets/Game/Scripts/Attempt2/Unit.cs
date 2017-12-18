using Prototype.Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Agent m_Agent;
        [SerializeField] private LineRenderer m_PathRenderer;
        public Agent Agent
        {
            get
            {
                return m_Agent;
            }
        }

        private void Update()
        {
            if(m_Agent.Path.Count > 0)
            {
                SetPathPoints();
            }
        }

        public List<Vector3> RemainingPositions()
        {
            List<Vector3> remainingPathPositions = new List<Vector3>();

            remainingPathPositions.Add(transform.position);
            int index = m_Agent.Path.IndexOf(m_Agent.OccupiedNode);

            for (int i = index + 1; i < m_Agent.Path.Count; i++)
            {
                remainingPathPositions.Add(m_Agent.Path[i].WorldPosition);
            }

            return remainingPathPositions;
        }

        void SetPathPoints()
        {
            List<Vector3> remainingPositions = RemainingPositions();
            m_PathRenderer.positionCount = remainingPositions.Count;

            for (int i = 0; i < m_PathRenderer.positionCount; i++)
            {
                m_PathRenderer.SetPosition(i, remainingPositions[i] + Vector3.up * 0.01f);
            }
        }

        public void TargetUnit(Unit targetUnit)
        {
            if (targetUnit == this)
            {
                return;
            }
            m_Agent.Path.Clear();

            Debug.Log("Targeting");
            m_PathRenderer.positionCount = 20;

            float step = 1.0f / m_PathRenderer.positionCount; 

            for (int i = 0; i < m_PathRenderer.positionCount; i++)
            {
                m_PathRenderer.SetPosition(i, Vector3.Lerp(transform.position, targetUnit.transform.position, step * i) + Vector3.up * 1f);
            }
        }
    }
}