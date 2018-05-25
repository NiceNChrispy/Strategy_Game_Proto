using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Reboot
{
    public class HexLevelEditor : CustomSceneView
    {
        private LevelData m_LevelBeingEdited;

        [MenuItem("Window/Level Editor")]
        static void Init()
        {
            CustomSceneView window = (HexLevelEditor)GetWindow(typeof(HexLevelEditor));
            window.titleContent = new GUIContent("Level Editor", "Level editor");
            window.Show();
        }

        private void OnEnable()
        {
            LoadScene();
            if (!m_LevelBeingEdited)
            {
                m_LevelBeingEdited = CreateInstance<LevelData>();
            }
        }

        protected override void OnGUI()
        {
            Plane plane = new Plane(Vector3.back, 0f);
            //Handles.DrawLine(Vector3.zero, Vector2.Scale(viewportPos,position.size));

            Ray ray = SceneCamera.ScreenPointToRay(Event.current.mousePosition);

            float enter = 0.0f;

            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Handles.DrawLine(Vector3.zero, hitPoint);
                Hex mouseHex = m_LevelBeingEdited.Layout.PixelToHex(hitPoint * 30).HexRound();
                Vector2 snappedMousePos = m_LevelBeingEdited.Layout.HexToPixel(mouseHex);
                Handles.DrawLine(Vector3.zero, snappedMousePos);
                GUI.Button(new Rect(0, 0, 100, 100), snappedMousePos.ToString());
                List<Vector2> points = m_LevelBeingEdited.Layout.PolygonCorners(mouseHex, 1);
                for (int i = 0; i < 6; i++)
                {
                    Handles.DrawLine(points[i], points[(i + 1) % 6]);
                }
            }

            for (int z = 0; z < 5; z++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        Hex hex = new Hex(x, y, z);
                    }
                }
            }

            base.OnGUI();
        }
    }
}