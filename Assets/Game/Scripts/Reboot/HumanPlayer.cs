using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class HumanPlayer : Player
    {
        private Selector<Unit> m_UnitSelector;
        [SerializeField] private LayerMask m_SelectionLayer;
        [SerializeField] private Camera m_Camera;
        [SerializeField] private Unit m_SelectedUnit;
        private Map m_Map;

        private void OnEnable()
        {
            m_UnitSelector = new Selector<Unit>(m_SelectionLayer);
        }

        private void Update()
        {
            UpdateUnitUnderCursor();

            if (Input.GetMouseButtonDown(0))
            {
                if (m_SelectedUnit != null)
                {

                }
                if (m_SelectedUnit != m_UnitSelector.CurrentSelectable)
                {
                    if (m_SelectedUnit != null)
                    {
                        m_SelectedUnit.Deselect();
                        m_SelectedUnit = null;
                    }
                    if (m_UnitSelector.CurrentSelectable != null)
                    {
                        m_SelectedUnit = m_UnitSelector.CurrentSelectable.SelectableComponent;
                        m_SelectedUnit.Select();
                    }
                }
            }
            //if(Input.GetMouseButtonDown(1))
            {
                if(m_SelectedUnit != null)
                {
                    Hex hitHex = GetHexAtCursor(m_GameManager.Layout);
                    List<DataStructures.AStarNode<Hex>> path = m_GameManager.GetPath(m_SelectedUnit.Position, hitHex);
                    if (path != null)
                    {
                        for (int i = 0; i < path.Count; i++)
                        {
                            Debug.DrawLine(m_GameManager.Layout.HexToPixel(path[i].Data), 
                                           m_GameManager.Layout.HexToPixel(path[(i + 1) % path.Count].Data));
                        }
                        Debug.DrawLine(m_SelectedUnit.transform.position, m_GameManager.Layout.HexToPixel(hitHex));
                    }
                }
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

        Hex GetHexAtCursor(Layout layout)
        {
            Plane checkPlane = new Plane(Vector3.back, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            Vector3 hitPoint = Vector3.zero;

            if (checkPlane.Raycast(ray, out enter))
            {
                hitPoint = ray.GetPoint(enter);

            }
            return layout.PixelToHex((Vector2)hitPoint).HexRound();
        }
    }
}