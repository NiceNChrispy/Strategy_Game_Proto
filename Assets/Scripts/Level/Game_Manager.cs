using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] Transform m_HighlighterTransform;
    [SerializeField] Transform m_TileTargeterTransform;

    [SerializeField] private LineRenderer m_LineRenderer;

    Unit2 m_SelectedUnit;
    GameTile m_SelectedTile;


    public void UnitSelected(Unit2 unit)
    {
        m_SelectedUnit = unit;
        m_HighlighterTransform.gameObject.SetActive(true);
        m_HighlighterTransform.position = unit.transform.position + (Vector3.up * 0.01f);
    }

    public void UnitDeselected()
    {
        m_TileTargeterTransform.gameObject.SetActive(false);
        m_HighlighterTransform.gameObject.SetActive(false);
    }

    public void TargetTile(GameTile tile)
    {
        if(m_SelectedUnit)
        {
            m_SelectedTile = tile;
            m_LineRenderer.positionCount = 2;
            m_LineRenderer.SetPosition(0, m_SelectedUnit.transform.position + (Vector3.up * 0.01f));
            m_LineRenderer.SetPosition(1, tile.transform.position + (Vector3.up * 0.01f));

            m_LineRenderer.enabled = true;
        }
    }
}