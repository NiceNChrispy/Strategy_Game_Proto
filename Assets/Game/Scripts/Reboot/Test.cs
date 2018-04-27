using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Map m_Map;
        private Hex m_MouseHex;

        private Dictionary<Hex, HexNode> m_HexDict;


        [SerializeField] private int VertexCount;
        [SerializeField] private int DictCount;

        [SerializeField] private string m_LevelName = "LEVEL.txt";
        [SerializeField] private string m_GraphName = "GRAPH.txt";

        Hex PathHex;
        [SerializeField] List<Vertex<Hex>> path;

        //private Graph<Hex> m_NavGraph;
        AStarNavGraph<Hex> m_NavGraph;

        string Path(string file) { return Application.dataPath + "/" + file; }

        private void Awake()
        {
            m_Layout = new Layout(m_IsFlat ? Layout.FLAT : Layout.POINTY, new Vector2(1f, 1f), Vector2.zero);

            Map loadedMap;
            if (!Load<Map>(Path(m_LevelName), out loadedMap))
            {
                throw new System.Exception("FAILED TO LOAD LEVEL");
            }
            Debug.Log("Loaded Map");
            BuildConnections(loadedMap);
        }

        void BuildConnections(Map map)
        {
            m_Map = new Map();
            m_NavGraph = new AStarNavGraph<Hex>();
            m_HexDict = new Dictionary<Hex, HexNode>();
            for (int i = 0; i < map.Hexes.Count; i++)
            {
                AddNode(map.Hexes[i]);
            }
            m_NavGraph = new AStarNavGraph<Hex>();
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

                bool isContained = m_Map.Contains(hitHex); //pre calculated so the dictionary doesnt try to add two elements with the same key

                if (Input.GetMouseButton(0) && !isContained)
                {
                    AddNode(hitHex);
                }
                if (Input.GetMouseButton(1) && isContained)
                {
                    RemoveNode(hitHex);
                }
                if(Input.GetKeyDown(KeyCode.P))
                {
                    PathHex = hitHex;
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
                if(Save(m_NavGraph, Path(m_GraphName)))
                {
                    Debug.Log(m_NavGraph.Vertices.Count);
                    Debug.Log(m_NavGraph.Edges.Count);
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
            VertexCount = m_NavGraph.Vertices.Count();
            DictCount = m_HexDict.Count;

            if(PathHex != null)
            {
                HexNode from, to;
                if(m_HexDict.TryGetValue(PathHex, out from) && m_HexDict.TryGetValue(m_MouseHex, out to))
                {
                    path = m_NavGraph.GetPath(from, to, ((x,y) => x.Distance(y)));
                    if(path != null)
                    {
                        //Debug.Log(string.Format("FOUND PATH OF {0} LENGTH", path.Count));
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            Debug.DrawLine(m_Layout.HexToPixel(path[i].Data), m_Layout.HexToPixel(path[i + 1].Data));
                        }
                    }
                }
            }
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
                    Hex startHex = m_NavGraph.Edges[i].vertex0.Data;
                    Hex endHex = m_NavGraph.Edges[i].vertex1.Data;
                    Vector2 start = m_Layout.HexToPixel(startHex);
                    Vector2 end = m_Layout.HexToPixel(endHex);
                    Vector2 startDir = (end - start);
                    Vector2 endDir = (start - end);

                    Gizmos.DrawRay(start, startDir * m_DrawScale * 0.5f);
                    Gizmos.DrawRay(end, endDir * m_DrawScale * 0.5f);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
            //DrawNeighbors();
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