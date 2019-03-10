using System.Collections.Generic;
using UnityEngine;

namespace SecondAttempt
{
    struct MouseData
    {
        public Hex HexPosition;
        public Vector2 WorldPosition;

        public MouseData(Hex hexPosition, Vector2 worldPosition)
        {
            HexPosition = hexPosition;
            WorldPosition = worldPosition;
        }
    }

    public class LevelEditor : MonoBehaviour
    {
        [SerializeField] Level m_LevelBeingEdited;
        [SerializeField] bool m_DrawGridLines;
        [SerializeField] int m_GridCount = 10;

        private MouseData m_MouseData;

        void Update()
        {
            UpdateMouseData();
            if(Input.GetMouseButton(0))
            {
                m_LevelBeingEdited.CreateTileAt(m_MouseData.WorldPosition);
            }
            if(Input.GetMouseButton(1))
            {
                m_LevelBeingEdited.RemoveTileAt(m_MouseData.WorldPosition);
            }
        }

        void UpdateMouseData()
        {
            Vector3 mousePos = Input.mousePosition;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane levelPlane = new Plane(Vector3.back, 0f);
            float enter = 0.0f;

            if(levelPlane.Raycast(mouseRay,out enter))
            {
                m_MouseData.WorldPosition = mouseRay.GetPoint(enter);
                m_MouseData.HexPosition = m_LevelBeingEdited.WorldPositionToHex(m_MouseData.WorldPosition);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_DrawGridLines && m_LevelBeingEdited)
            {
                for (int q = -m_GridCount; q <= m_GridCount; q++)
                {
                    for (int r = -m_GridCount; r <= m_GridCount; r++)
                    {
                        for (int s = -m_GridCount; s <= m_GridCount; s++)
                        {
                            if (q + r + s == 0)
                            {
                                Hex hex = new Hex(q, r, s);
                                GizmoDrawHex(hex);
                            }
                        }
                    }
                }
            }
        }

        private void GizmoDrawHex(Hex hex)
        {
            List<Vector2> hexPoints = Layout.Default.PolygonCorners(hex);
            for (int i = 0; i < 6; i++)
            {
                Gizmos.DrawLine(m_LevelBeingEdited.transform.TransformPoint(hexPoints[i]), m_LevelBeingEdited.transform.TransformPoint(hexPoints[(i + 1) % 6]));
            }
        }
    }
}
