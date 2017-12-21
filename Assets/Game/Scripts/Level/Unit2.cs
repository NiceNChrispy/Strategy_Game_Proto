using Navigation;
using System;
using UnityEngine;

public class Unit2 : MonoBehaviour
{
    [SerializeField] private Agent m_Agent;
    [SerializeField] private LineRenderer m_LineRenderer;

    public Agent Agent
    {
        get
        {
            return m_Agent;
        }
    }

    public Unit2 TargetUnit;

    public void Select()
    {
        Game_Manager.Instance.SelectUnit(this);
    }

    public void SetTarget(Unit2 target)
    {
        TargetUnit = target;
    }

    public void ClearTarget()
    {
        m_LineRenderer.positionCount = 0;
        TargetUnit = null;
    }

    private void Update()
    {
        m_LineRenderer.positionCount = 0;
        if (TargetUnit != null)
        {
            DrawUnitTarget();
        }
        if (Agent.HasPath)
        {
            DrawAgentPath();
        }
    }

    public void DrawUnitTarget()
    {
        int pointCount = 40;
        m_LineRenderer.positionCount = pointCount;
        float step = 1.0f / (pointCount - 1);

        for (int i = 0; i < pointCount; i++)
        {
            float t = i * step;
            float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
            m_LineRenderer.SetPosition(i, Vector3.Lerp(transform.position, TargetUnit.transform.position, t) +
                                        (Vector3.Lerp(Vector3.up * 0.5f, Vector3.up * Mathf.Sqrt(distance) * 0.5f, Mathf.Sin(Mathf.LerpAngle(0, Mathf.PI, t)))));
        }
    }

    public void DrawAgentPath()
    {
        int startIndex = 0;

        for (int i = 0; i < m_Agent.Path.Count; i++)
        {
            if (m_Agent.Path[i] == m_Agent.ActiveNode)
            {
                startIndex = i + 1;
                break;
            }
        }

        m_LineRenderer.positionCount = m_Agent.Path.Count - startIndex + 1;

        m_LineRenderer.SetPosition(0, transform.position + new Vector3(0, 0.01f, 0));

        for (int i = startIndex; i < m_Agent.Path.Count; i++)
        {
            m_LineRenderer.SetPosition(i - startIndex + 1, m_Agent.Path[i].Data.Position + new Vector3(0, 0.01f, 0));
        }
    }
}