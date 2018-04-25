using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Reboot
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private bool m_IsFlat;

        [SerializeField] private float m_PlaneOffset;
        [SerializeField] private Vector3 m_PlaneNormal;
        [SerializeField] private Material m_LineMat;
        [SerializeField] private Material m_LineMat2;
        [SerializeField] private float m_DrawScale = 1.0f;

        Layout m_Layout;
        Dictionary<Hex, IVertex<Hex>> nodes;
        Map m_Map;
        private Hex m_MouseHex;

        private Dictionary<Hex, HexNode> m_HexDict;

        private Graph<Hex> m_NavGraph;

        [SerializeField] private int VertexCount;
        [SerializeField] private int DictCount;

        private void Awake()
        {
            m_Layout = new Layout(m_IsFlat ? Layout.FLAT : Layout.POINTY, new Vector2(1f, 1f), Vector2.zero);
            LoadMap();
            m_HexDict = new Dictionary<Hex, HexNode>();
            m_NavGraph = new Graph<Hex>();

            for (int i = 0; i < m_Map.Hexes.Count; i++)
            {
                AddNode(m_Map.Hexes[i]);
            }
        }

        private void SaveMap()
        {
            string filePath = Application.dataPath + "/LEVEL.txt";
            StreamWriter sw = new StreamWriter(filePath, false);
            string map = JsonUtility.ToJson(m_Map, true);
            sw.Write(map);
            sw.Close();
            Debug.Log("Saved Map");
        }

        private void LoadMap()
        {
            string filePath = Application.dataPath + "/LEVEL.txt";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                m_Map = JsonUtility.FromJson<Map>(sr.ReadToEnd());
                sr.Close();
                Debug.Log("Loaded Map");
            }
            else
            {
                m_Map = new Map();
            }
        }

        private void Update()
        {
            Plane checkPlane = new Plane(m_PlaneNormal, m_PlaneOffset);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (checkPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Hex hitHex = m_Layout.PixelToHex((Vector2)hitPoint).HexRound();

                m_MouseHex = hitHex;

                bool isContained = m_Map.Contains(hitHex); //pre calculated so the dictionary doesnt try to add two elements with the same key

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
                SaveMap();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadMap();
            }
            VertexCount = m_NavGraph.Vertices.Count();
            DictCount = m_HexDict.Count;
        }

        private void AddNode(Hex hex)
        {
            m_Map.Add(hex);
            HexNode node = new HexNode(hex);
            m_HexDict.Add(hex, node);
            m_NavGraph.Add(node);

            foreach (Hex neighbor in hex.AllNeighbors())
            {
                if (neighbor != null)
                {
                    HexNode neighborNode;
                    if (m_HexDict.TryGetValue(neighbor, out neighborNode))
                    {
                        m_NavGraph.AddEdge(node, neighborNode);
                    }
                }
            }
        }

        private void RemoveNode(Hex hex)
        {
            m_Map.Remove(hex);

            HexNode node;

            if (m_HexDict.TryGetValue(hex, out node))
            {
                m_NavGraph.Remove(node);
                m_HexDict.Remove(hex);
            }
        }

        private void DrawNeighbors()
        {
            if (m_NavGraph != null)
            {
                for (int i = 0; i < m_NavGraph.Edges.Count; i++)
                {
                    Hex startHex = m_NavGraph.Edges[i].vertex0.Value;
                    Hex endHex =   m_NavGraph.Edges[i].vertex1.Value;

                    Gizmos.DrawLine(m_Layout.HexToPixel(startHex), m_Layout.HexToPixel(endHex));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
            DrawNeighbors();
        }

        private void OnPostRender()
        {
            Draw();
        }

        private void Draw()
        {
            if (m_Map != null)
            {
                m_Layout.Orientation = m_IsFlat ? Layout.FLAT : Layout.POINTY;

                foreach (Hex hex in m_Map.Hexes)
                {
                    List<Vector2> points = m_Layout.PolygonCorners(hex, m_DrawScale);

                    GL.Begin(GL.LINES);
                    m_LineMat.SetPass(0);
                    GL.Color(Color.white);
                    for (int i = 0; i < 6; i++)
                    {
                        GL.Vertex((Vector3)points[i]);
                        GL.Vertex((Vector3)points[(i + 1) % 6]);
                    }
                    GL.End();
                }
                List<Vector2> mousePoints = m_Layout.PolygonCorners(m_MouseHex, m_DrawScale);
                GL.Begin(GL.LINES);
                m_LineMat2.SetPass(0);
                GL.Color(Color.green);
                for (int i = 0; i < 6; i++)
                {
                    GL.Vertex((Vector3)mousePoints[i]);
                    GL.Vertex((Vector3)mousePoints[(i + 1) % 6]);
                }
                GL.End();
            }
        }
    }
}