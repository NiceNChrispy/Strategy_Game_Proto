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

    public void TargetTile(HexTile tile)
    {
        if (m_SelectedUnit && tile != m_SelectedTile)
        {
            m_SelectedTile = tile;

            Node unitNode = m_SelectedUnit.Agent.ActiveNode;
            Node tileNode = m_NavMap[tile.X, tile.Y];

            m_NavMap.IndexOf(tileNode);

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

            m_HighlighterTransform.gameObject.SetActive(false);

            m_LineRenderer.enabled = true;
        }
    }

    public void TileAction(HexTile tile)
    {
        if(m_SelectedUnit)
        {
            m_SelectedUnit.MoveTo(m_NavMap[tile.X, tile.Y]);
        }
    }

    public void Update()
    {
        if(m_SelectedUnit != null && m_SelectedUnit.Agent.HasPath)
        {
            
        }
    }
}