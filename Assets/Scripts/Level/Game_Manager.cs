using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : Singleton<Game_Manager>
{
    [SerializeField] Transform m_HighlighterTransform;
    [SerializeField] Transform m_TileTargeterTransform;

    [SerializeField] private LineRenderer m_LineRenderer;

    [SerializeField] private Navigation.NavGraph m_NavGraph;

    Unit2 m_SelectedUnit;
    HexTile m_SelectedTile;

    public void SelectUnit(Unit2 unit)
    {
        if (unit != m_SelectedUnit)
        {
            m_SelectedUnit = unit;
            m_HighlighterTransform.gameObject.SetActive(true);
            m_HighlighterTransform.position = unit.transform.position + (Vector3.up * 0.01f);
        }
    }

    public void DeselectUnit()
    {
        m_TileTargeterTransform.gameObject.SetActive(false);
        m_HighlighterTransform.gameObject.SetActive(false);
    }

    public void TargetTile(HexTile tile)
    {
        if(m_SelectedUnit && tile != m_SelectedTile)
        {
            m_SelectedTile = tile;

            NavNode unitNode = m_SelectedUnit.ActiveNode;

            List<NavNode> path = m_NavGraph.GetPath(unitNode, m_NavGraph[tile.Y, tile.X]);

            m_LineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                m_LineRenderer.SetPosition(i, path[i].Position + (Vector3.up * 0.01f));
            }

            m_TileTargeterTransform.gameObject.SetActive(true);
            m_TileTargeterTransform.position = m_SelectedTile.transform.position;

            m_HighlighterTransform.gameObject.SetActive(false);

            m_LineRenderer.enabled = true;
        }
    }

    public void TileAction(HexTile tile)
    {
        if(m_SelectedUnit)
        {
            m_SelectedUnit.MoveTo(m_NavGraph[tile.Y, tile.X]);
        }
    }

    public void Update()
    {
        if(m_SelectedUnit != null && m_SelectedUnit.HasPath)
        {

        }
    }
}