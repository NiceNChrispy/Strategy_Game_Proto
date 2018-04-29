using UnityEngine;

namespace Reboot
{
    public class HumanPlayer : Player
    {
        private Selector<Unit> m_UnitSelector;
        [SerializeField] private LayerMask m_SelectionLayer;
        [SerializeField] private Camera m_Camera;
        [SerializeField] private Unit m_SelectedUnit;

        [SerializeField] private MapData m_Level;

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
                        m_SelectedUnit = m_UnitSelector.CurrentSelectable.Component;
                        m_SelectedUnit.Select();
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

        void GetHexAtCursor()
        {

        }
    }
}