using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Reboot
{
    public class Test : MonoBehaviour
    {
        Layout m_Layout;

        [SerializeField] private bool m_IsFlat;

        [SerializeField] private float m_PlaneOffset;
        [SerializeField] private Vector3 m_PlaneNormal;
        [SerializeField] private Material m_LineMat;
        [SerializeField] private Material m_LineMat2;
        [SerializeField] private float m_DrawScale = 1.0f;
        Dictionary<Hex, HexNode> nodes;
        Map m_Map;
        private Hex m_MouseHex;

        private void Awake()
        {
            m_Layout = new Layout(m_IsFlat ? Layout.FLAT : Layout.POINTY, new Vector2(1f, 1f), Vector2.zero);
            LoadMap();
            nodes = new Dictionary<Hex, HexNode>();

            foreach (Hex hex in m_Map.Tiles)
            {
                HexNode hNode = new HexNode();
                hNode.Data = hex;
                nodes.Add(hex, hNode);
            }

            Debug.Log(nodes.Count);

            foreach (HexNode hexnode in nodes.Values)
            {
                List<INavigationNode<Hex>> neighbouringNodes = new List<INavigationNode<Hex>>();
                for (int i = 0; i < 6; i++)
                {
                    Hex neighbourHex = hexnode.Data.Neighbor(i);
                    if (m_Map.Contains(neighbourHex))
                    {
                        HexNode outNode;
                        if (nodes.TryGetValue(neighbourHex, out outNode))
                        {
                            neighbouringNodes.Add(outNode);
                        }
                    }
                }
                hexnode.Connections = neighbouringNodes;
            }
        }

        private void DrawConnections()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            foreach (HexNode hexNode in nodes.Values)
            {
                foreach (HexNode connection in hexNode.Connections)
                {
                    Gizmos.DrawLine(m_Layout.HexToPixel(connection.Data), m_Layout.HexToPixel(hexNode.Data));
                }
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
            StreamReader sr = new StreamReader(filePath);
            m_Map = JsonUtility.FromJson<Map>(sr.ReadToEnd());
            sr.Close();
            Debug.Log("Loaded Map");
        }

        private void Update()
        {
            Plane checkPlane = new Plane(m_PlaneNormal, m_PlaneOffset);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (checkPlane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);
                //Move your cube GameObject to the point where you clicked
                Hex hitHex = m_Layout.PixelToHex((Vector2)hitPoint).HexRound();
                bool contains = m_Map.Contains(hitHex);

                m_MouseHex = hitHex;

                if (Input.GetMouseButton(0) && !contains)
                {
                    m_Map.Add(hitHex);
                }
                if (Input.GetMouseButton(1) && contains)
                {
                    m_Map.Remove(hitHex);
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
        }

        void GenHex(int range)
        {
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
                {
                    m_Map.Add(new Hex(dx, dy, -dx - dy));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
            DrawConnections();
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //SAVING
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXX

        private void OnPostRender()
        {
            Draw();
        }

        private void Draw()
        {
            if (m_Map != null)
            {
                m_Layout.Orientation = m_IsFlat ? Layout.FLAT : Layout.POINTY;

                foreach (Hex hex in m_Map.Tiles)
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