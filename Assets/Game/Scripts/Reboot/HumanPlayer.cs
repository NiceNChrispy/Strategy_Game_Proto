using DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class HumanPlayer : Player
    {
        private Selector<Unit> m_UnitSelector;
        [SerializeField] private LayerMask m_SelectionLayer;
        [SerializeField] private Camera m_Camera;

        private void OnEnable()
        {
            m_UnitSelector = new Selector<Unit>(m_SelectionLayer);
        }

        private void Update()
        {
            UpdateUnitUnderCursor();

            if (m_SelectedUnit != null && m_UnitSelector.CurrentSelectable != null)
            {
                Unit targetUnit = m_UnitSelector.CurrentSelectable.SelectableComponent;

                if (!m_Units.Contains(targetUnit))
                {
                    Debug.DrawLine(m_SelectedUnit.transform.position, targetUnit.transform.position, Color.red);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (m_SelectedUnit != null)
                {

                }
                if (m_SelectedUnit != m_UnitSelector.CurrentSelectable)
                {
                    if (m_SelectedUnit != null)
                    {
                        m_SelectedUnit.OnFinishMove -= UpdateUnitsMoveableTiles;
                        DeselectUnit();
                    }
                    if (m_UnitSelector.CurrentSelectable != null && m_Units.Contains(m_UnitSelector.CurrentSelectable.SelectableComponent))
                    {
                        SelectUnit(m_UnitSelector.CurrentSelectable.SelectableComponent);
                    }
                }
            }
            if (m_SelectedUnit != null)
            {
                Hex hitHex = GetHexAtCursor();

                List<AStarNode<Hex>> path = m_GameManager.GetPath(m_SelectedUnit.Position, hitHex);
                if(path != null)
                {
                    m_Path = path.Where(x => m_MoveableTiles.Contains(x)).ToList();
                }

                //TODO Clamp Path to accessible nodes
                if (m_Path != null)
                {
                    for (int i = 0; i < m_Path.Count - 1; i++)
                    {
                        Debug.DrawLine(m_GameManager.HexToWorld(m_Path[i].Data),
                                       m_GameManager.HexToWorld(m_Path[i + 1].Data));
                    }
                    //Debug.DrawLine(m_SelectedUnit.transform.position, m_GameManager.Layout.HexToPixel(hitHex));
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (m_SelectedUnit != null && Path != null)
                        {
                            m_SelectedUnit.Move(Path, m_GameManager);
                            m_SelectedUnit.OnFinishMove += UpdateUnitsMoveableTiles;
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (m_Path != null)
            {
                GUILayout.Label(string.Format("Path Length: {0}", m_Path.Count - 1));
            }
        }

        void UpdateUnitUnderCursor()
        {
            Vector3 cursorPosition;
            cursorPosition.x = Input.mousePosition.x;
            cursorPosition.y = Input.mousePosition.y;
            cursorPosition.z = m_Camera.nearClipPlane;

            Ray selectionRay = m_Camera.ScreenPointToRay(cursorPosition);
            m_UnitSelector.UpdateSelection(selectionRay);
        }

        Hex GetHexAtCursor()
        {
            Plane checkPlane = new Plane(Vector3.back, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            Vector3 hitPoint = Vector3.zero;

            if (checkPlane.Raycast(ray, out enter))
            {
                hitPoint = ray.GetPoint(enter);
            }
            return m_GameManager.WorldToHex((Vector2)hitPoint);
        }
    }
}