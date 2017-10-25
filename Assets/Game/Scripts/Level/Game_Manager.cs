using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

public class Game_Manager : Singleton<Game_Manager>
{
    [SerializeField] private Transform m_HighlighterTransform;
    [SerializeField] private Transform m_TileTargeterTransform;
    [SerializeField] private LineRenderer m_LineRenderer;
    [SerializeField] private Map m_NavMap;

    [SerializeField] Unit2 m_SelectedUnit;
    [SerializeField] HexTile m_SelectedTile;
    [SerializeField] HexTile m_TileUnderCursor;

    bool hasOrder;

    public void SelectUnit(Unit2 unit)
    {
        if (unit != m_SelectedUnit)
        {
            hasOrder = false;
            m_SelectedUnit = unit;
            m_HighlighterTransform.gameObject.SetActive(true);
            m_HighlighterTransform.position = unit.transform.position + (Vector3.up * 0.01f);
        }
    }

    public void UpdateTileUnderCursor(HexTile tile)
    {
        if (m_SelectedUnit && tile != m_TileUnderCursor)
        {
            m_TileUnderCursor = tile;
            TargetTile();
        }
    }

    public void ClearTile()
    {
        m_SelectedTile = null;
        m_TileUnderCursor = null;

        m_TileTargeterTransform.gameObject.SetActive(false);

        if(!hasOrder)
        {
            m_LineRenderer.positionCount = 0;
        }
    }

    void TargetTile()
    {
        bool isNull = m_SelectedTile == null;
        m_SelectedTile = m_TileUnderCursor;

        Node unitNode = m_SelectedUnit.Agent.ActiveNode;
        Node tileNode = m_NavMap[m_SelectedTile.X, m_SelectedTile.Y];

        m_NavMap.IndexOf(tileNode);

        if (!hasOrder)
        {
            Path path = m_NavMap.GetPath(unitNode, tileNode);

            if (path != null)
            {
                m_LineRenderer.positionCount = path.Count;

                for (int i = 0; i < path.Count; i++)
                {
                    m_LineRenderer.SetPosition(i, path[i].Position + (Vector3.up * 0.01f));
                }
            }
            m_TileTargeterTransform.gameObject.SetActive(true);
            m_TileTargeterTransform.position = m_SelectedTile.transform.position;
        }

        m_HighlighterTransform.gameObject.SetActive(false);
        m_LineRenderer.enabled = true;
    }

    public void TileAction(HexTile tile)
    {
        if(m_SelectedUnit)
        {
            m_SelectedUnit.MoveTo(m_NavMap[tile.X, tile.Y], ClearOrder);
            hasOrder = true;
            m_HighlighterTransform.gameObject.SetActive(false);
            m_TileTargeterTransform.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if(m_SelectedUnit != null && hasOrder)
        {
            DrawUnitPath();
        }
    }

    private void ClearOrder()
    {
        hasOrder = false;
        TargetTile();
    }

    public void DrawUnitPath()
    {
        if (!m_SelectedUnit.Agent.HasPath)
        {
            return;
        }

        Path unitPath = m_SelectedUnit.Agent.Path;

        int startIndex = 0;

        for (int i = 0; i < unitPath.Count; i++)
        {
            if(unitPath[i] == m_SelectedUnit.Agent.NextNode)
            {
                startIndex = i;
                break;
            }
        }

        m_LineRenderer.positionCount = unitPath.Count - startIndex + 1;

        m_LineRenderer.SetPosition(0, m_SelectedUnit.transform.position + (Vector3.up * 0.01f));

        for (int i = startIndex; i < unitPath.Count; i++)
        {
            m_LineRenderer.SetPosition(i - startIndex + 1, unitPath[i].Position + (Vector3.up * 0.01f));
        }
    }
}