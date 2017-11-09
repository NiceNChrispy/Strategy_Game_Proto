using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Controller : MonoBehaviour
{
    [SerializeField] private Interactor m_Interactor;
    [SerializeField] private Text m_CurrentInteractableText;

    [SerializeField] private Unit2 m_SelectedUnit;
    [SerializeField] private Unit2 m_TargetUnit;
    [SerializeField] private Unit2 m_PreviousTargetUnit;
    [SerializeField] private HexTile m_TargetTile;
    [SerializeField] private HexTile m_PreviousTargetTile;

    [SerializeField] private Map m_NavMap;

    void Update()
    {
        m_CurrentInteractableText.text = m_Interactor.Current ? m_Interactor.Current.gameObject.name : "NONE";

        if(m_Interactor.Current)
        {
            m_TargetUnit = m_Interactor.Current.GetComponent<Unit2>();
            m_TargetTile = m_Interactor.Current.GetComponent<HexTile>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (m_TargetUnit)
            {
                if (m_SelectedUnit != m_TargetUnit)
                {
                    if (m_SelectedUnit)
                    {
                        m_SelectedUnit.ClearTarget();
                        m_SelectedUnit.Agent.ClearDestination();
                    }
                    m_SelectedUnit = m_TargetUnit;
                }
            }
        }

        if (m_SelectedUnit)
        {
            if (m_TargetTile && m_TargetTile != m_PreviousTargetTile)
            {
                m_SelectedUnit.ClearTarget();
                if(!m_SelectedUnit.Agent.IsPathing)
                {
                    m_SelectedUnit.Agent.SetDestination(m_NavMap[m_TargetTile.X, m_TargetTile.Y]);
                }
            }
            if(m_TargetUnit && m_TargetUnit != m_PreviousTargetUnit)
            {
                m_SelectedUnit.Agent.ClearDestination();
                m_SelectedUnit.SetTarget(m_TargetUnit);
            }
        }

        if (Input.GetMouseButtonUp(1) && m_SelectedUnit)
        {
            if(m_TargetTile)
            {
                if (!m_SelectedUnit.Agent.IsPathing)
                {
                    m_SelectedUnit.Agent.BeginPathing();
                }
            }
            else if(m_TargetUnit)
            {
                Debug.Log("ATTACK");
            }
        }

        m_PreviousTargetUnit = m_TargetUnit;
        m_PreviousTargetTile = m_TargetTile;
    }
}