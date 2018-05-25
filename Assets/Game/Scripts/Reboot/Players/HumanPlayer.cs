using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class HumanPlayer : Player
    {
        private Selector<Unit> m_UnitSelector;
        private Selector<Tile> m_TileSelector;

        [SerializeField] private LayerMask m_SelectionLayer;
        [SerializeField] private Camera m_Camera;

        private void OnEnable()
        {
            m_UnitSelector = new Selector<Unit>(m_SelectionLayer);
            m_TileSelector = new Selector<Tile>(m_SelectionLayer);
        }



        private void Update()
        {
            if (m_IsMyTurn && !m_IsBusy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    UpdateUnitUnderCursor();

                    Unit targetUnit = m_UnitSelector.CurrentSelectable != null ? m_UnitSelector.CurrentSelectable.Data : null;
                    if (targetUnit != null && m_SelectedUnit != targetUnit && IsMyUnit(targetUnit))
                    {
                        DeselectSelectedUnit();
                        SelectUnit(targetUnit);
                    }

                    Tile targetTile = m_TileSelector.CurrentSelectable != null ? m_TileSelector.CurrentSelectable.Data : null;
                    if (targetTile != null && (INavNode<Hex>)targetTile != m_TargetTile)
                    {
                        TargetTile(targetTile);
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (m_SelectedUnit != null)
            {
                GUILayout.Label(string.Format("AP: {0}/{1}", m_ActionPoints, m_MaxActionPoints));
                GUILayout.Label(string.Format("Selected Unit: {0}", m_SelectedUnit.name));
                GUILayout.Label(string.Format("Health: {0}/{1}", m_SelectedUnit.Health, m_SelectedUnit.MaxHealth));
                GUILayout.Label(string.Format("Movement Range: {0}", m_SelectedUnit.MovementRange));
                if(m_CurrentAttackIndex != -1)
                {
                    GUILayout.Label(string.Format("Attack Range: {0}", m_SelectedUnit.Attacks[m_CurrentAttackIndex].Range));
                }
                if (m_Path.Count > 0)
                {
                    GUILayout.Label(string.Format("Path Length: {0}", m_Path.Count - 1));
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
            m_TileSelector.UpdateSelection(selectionRay);
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
            return GameManager.Instance.WorldToHex((Vector2)hitPoint);
        }
    }
}