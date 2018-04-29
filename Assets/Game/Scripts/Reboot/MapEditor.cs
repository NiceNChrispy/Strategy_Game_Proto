using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using DataStructures;

namespace Reboot
{
    public class MapEditor : MonoBehaviour
    {
        [SerializeField] private bool m_IsFlat;

        [SerializeField] private float m_PlaneOffset;
        [SerializeField] private Vector3 m_PlaneNormal;
        [SerializeField] private Material m_HexMat;
        [SerializeField] private Material m_MouseHexMat;
        [SerializeField] private Material m_PathMat;
        [SerializeField] private float m_DrawScale = 1.0f;

        Layout m_Layout;
        Map m_Map;
        private Hex m_MouseHex;

        [SerializeField] private string m_LevelName = "LEVEL.txt";
        [SerializeField] private string m_GraphName = "GRAPH.txt";

        Hex PathHex;
        [SerializeField] List<AStarNode<Hex>> m_Path;

        DataStructures.NavGraph<Hex> m_NavGraph;
        [SerializeField] private int m_Range = 1;

        string Path(string file) { return Application.dataPath + "/" + file; }

        private void Start()
        {
            m_Layout = new Layout(m_IsFlat ? Layout.FLAT : Layout.POINTY, new Vector2(1f, 1f), Vector2.zero);

            Map loadedMap;
            if (!Load(Path(m_LevelName), out loadedMap))
            {
                throw new System.Exception("FAILED TO LOAD LEVEL");
            }
            Debug.Log("Loaded Map");
            BuildConnections(loadedMap);
        }

        void BuildConnections(Map map)
        {
            m_Map = new Map();
            m_NavGraph = new NavGraph<Hex>();
            for (int i = 0; i < map.Hexes.Count; i++)
            {
                AddNode(map.Hexes[i]);
            }
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
            Plane checkPlane = new Plane(m_PlaneNormal, m_PlaneOffset);
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
                if (Input.GetKeyDown(KeyCode.P))
                {
                    PathHex = hitHex;
                    Debug.Log(m_NavGraph.Nodes.Count);
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Save(m_Map, Path(m_LevelName)))
                {
                    Debug.Log("Saved Map");
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (Save(m_NavGraph, Path(m_GraphName)))
                {
                    Debug.Log("Saved Graph");
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Map loadedMap;
                if (!Load(Path(m_LevelName), out loadedMap))
                {
                    throw new System.Exception("FAILED TO LOAD LEVEL");
                }
                Debug.Log("Loaded Map");
                BuildConnections(loadedMap);
            }
        }

        private void DrawMouseRange()
        {
            if(m_Map.Contains(m_MouseHex))
            {
                List<AStarNode<Hex>> nodesInRange = m_NavGraph.GetNodesInRange(m_MouseHex, m_Range);
                if (nodesInRange != null)
                {
                    GL.Begin(GL.LINES);
                    m_PathMat.SetPass(0);
                    for (int i = 0; i < nodesInRange.Count; i++)
                    {
                        List<Vector2> points = m_Layout.PolygonCorners(nodesInRange[i].Data, m_DrawScale);
                        for (int j = 0; j < 6; j++)
                        {
                            GL.Vertex((Vector3)points[j]);
                            GL.Vertex((Vector3)points[(j + 1) % 6]);
                        }
                    }
                    GL.End();
                }
            }
        }

        private void AddNode(Hex hex)
        {
            m_Map.Add(hex);
            m_NavGraph.AddNode(hex);

            foreach (Hex neighbor in hex.AllNeighbors())
            {
                if (m_Map.Contains(neighbor))
                {
                    m_NavGraph.AddUndirectedEdge(hex, neighbor, 1);
                }
            }
        }

        private void RemoveNode(Hex hex)
        {
            m_Map.Remove(hex);
            m_NavGraph.Remove(hex);
        }

        private void DrawNeighbors()
        {
            if (m_NavGraph != null)
            {
                foreach (GraphNode<Hex> node in m_NavGraph.Nodes)
                {
                    Vector2 start = m_Layout.HexToPixel(node.Data);
                    Vector2 end;

                    foreach (GraphNode<Hex> neighbor in node.Neighbors)
                    {
                        end = m_Layout.HexToPixel(neighbor.Data);
                        Vector2 dir = (end - start);

                        Gizmos.DrawRay(start, dir * 0.5f * m_DrawScale);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
        }

        private void DrawPath()
        {
            if (PathHex != null)
            {
                if (m_Map.Contains(m_MouseHex) && m_Map.Contains(PathHex))
                {
                    m_Path = m_NavGraph.GetPath(m_MouseHex, PathHex, ((x, y) => x.Distance(y)));
                    if (m_Path != null)
                    {
                        GL.Begin(GL.LINES);
                        m_PathMat.SetPass(0);
                        for (int i = 0; i < m_Path.Count - 1; i++)
                        {
                            GL.Vertex((Vector3)m_Layout.HexToPixel(m_Path[i].Data));
                            GL.Vertex((Vector3)m_Layout.HexToPixel(m_Path[i + 1].Data));
                        }
                        GL.End();
                    }
                }
            }
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
                DrawPath();

                foreach (Hex hex in m_Map.Hexes)
                {
                    List<Vector2> points = m_Layout.PolygonCorners(hex, m_DrawScale);

                    GL.Begin(GL.LINES);
                    m_HexMat.SetPass(0);
                    for (int i = 0; i < 6; i++)
                    {
                        GL.Vertex((Vector3)points[i]);
                        GL.Vertex((Vector3)points[(i + 1) % 6]);
                    }
                    GL.End();
                }
                List<Vector2> mousePoints = m_Layout.PolygonCorners(m_MouseHex, m_DrawScale);
                GL.Begin(GL.LINES);
                m_MouseHexMat.SetPass(0);
                for (int i = 0; i < 6; i++)
                {
                    GL.Vertex((Vector3)mousePoints[i]);
                    GL.Vertex((Vector3)mousePoints[(i + 1) % 6]);
                }
                GL.End();
                DrawMouseRange();
            }
        }
    }
}