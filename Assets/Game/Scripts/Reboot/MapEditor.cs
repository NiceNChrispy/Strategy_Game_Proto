using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using DataStructures;

namespace Reboot
{
    public class MapEditor : MonoBehaviour
    {
        [SerializeField] private float m_DrawScale = 1.0f;

        Layout m_Layout;
        Map<Hex> m_Map;
        private Hex m_MouseHex;

        [SerializeField] private string m_MapName = "LEVEL.txt";
        [SerializeField] private string m_GraphName = "GRAPH.txt";

        string Path(string file) { return Application.dataPath + "/" + file; }

        private void Start()
        {
            m_Layout = new Layout(Layout.FLAT, new Vector2(1f, 1f), Vector2.zero);
            LoadMap();
        }

        private bool Save(object obj, string filepath)
        {
            StreamWriter sw = new StreamWriter(filepath, false);
            string data = JsonUtility.ToJson(obj, true);
            sw.Write(data);
            sw.Close();
            return true;
        }

        private bool Load<T>(string filePath, out T loaded)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                loaded = JsonUtility.FromJson<T>(sr.ReadToEnd());
                sr.Close();
                return true;
            }
            loaded = default(T);
            return false;
        }

        private void Update()
        {
            Plane checkPlane = new Plane(Vector3.back, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (checkPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Hex hitHex = m_Layout.PixelToHex((Vector2)hitPoint).HexRound();

                m_MouseHex = hitHex;

                bool isContained = m_Map.Contains(hitHex);

                if (Input.GetMouseButton(0) && !isContained)
                {
                    AddNode(hitHex);
                }
                if (Input.GetMouseButton(1) && isContained)
                {
                    RemoveNode(hitHex);
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Save(m_Map, Path(m_MapName)))
                {
                    Debug.Log("Saved Map");
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadMap();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label(string.Format("Map: {0}", m_MapName));
            if (GUILayout.Button("Save"))
            {
                Save(m_Map, Path(m_MapName));
            }
            if (GUILayout.Button("Load"))
            {
                LoadMap();
            }
        }

        private void LoadMap()
        {
            Map<Hex> loadedMap;
            if (!Load(Path(m_MapName), out loadedMap))
            {
                throw new System.Exception("FAILED TO LOAD LEVEL");
            }
            else
            {
                m_Map = loadedMap;
                Debug.Log(string.Format("Loaded {0} with {1} hexes", m_MapName, m_Map.Contents.Count));
            }
        }

        private void AddNode(Hex hex)
        {
            m_Map.Add(hex);
        }

        private void RemoveNode(Hex hex)
        {
            m_Map.Remove(hex);
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
        }

        private void Draw()
        {
            if (m_Map != null)
            {
                foreach (Hex hex in m_Map.Contents)
                {
                    List<Vector2> points = m_Layout.PolygonCorners(hex, m_DrawScale);
                    List<Vector2> points2 = m_Layout.PolygonCorners(hex, m_DrawScale * 0.95f);

                    for (int i = 0; i < 6; i++)
                    {
                        Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
                        Gizmos.DrawLine(points2[i], points2[(i + 1) % 6]);
                    }
                }
                Gizmos.color = m_Map.Contains(m_MouseHex) ? Color.red : Color.green;
                List<Vector2> mousePoints = m_Layout.PolygonCorners(m_MouseHex, m_DrawScale);
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(mousePoints[i], mousePoints[(i + 1) % 6]);
                }
            }
        }
    }
}