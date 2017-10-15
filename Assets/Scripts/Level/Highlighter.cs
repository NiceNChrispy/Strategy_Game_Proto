using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private Transform m_HighlightObject;

    [SerializeField, ReadOnly] private string m_HighlightedUnitName;

    void Highlight(Unit2 unit, bool highlight)
    {
        m_HighlightedUnitName = highlight ? unit.name : "NONE";
        m_HighlightObject.transform.position = unit.transform.position;
        m_HighlightObject.gameObject.SetActive(highlight);
    }
}