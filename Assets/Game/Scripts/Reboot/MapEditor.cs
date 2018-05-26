using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reboot
{
    public class MapEditor : MonoBehaviour
    {
        [SerializeField] private float m_DrawScale = 1.0f;
        [SerializeField] private HashSet<Hex> m_RawMapData;
        [SerializeField] private Tile m_TileObject;

        private Renderer m_PreviewRenderer;
        private Tile m_TilePreview;
        Dictionary<Hex, Tile> m_Tiles;
        [SerializeField] int height = 0;
        Layout m_Layout;
        private Hex m_MouseHex;

        [SerializeField] private string m_MapName = "LEVEL.txt";

        string Path(string file) { return Application.persistentDataPath + "/Maps/" + file; }

        private void OnEnable()
        {
            m_Layout = new Layout(Layout.FLAT, new Vector2(1f, 1f), Vector2.zero);
            m_TilePreview = Instantiate(m_TileObject);
            m_TilePreview.hideFlags = HideFlags.HideAndDontSave;
            m_Tiles = new Dictionary<Hex, Tile>();
            m_PreviewRenderer = m_TilePreview.GetComponent<Renderer>();
            m_PreviewRenderer.material = new Material(Shader.Find("Unlit/Color"));
            LoadOrCreateMap();
        }

        private void SaveMap()
        {
            MapData mapData = new MapData(m_RawMapData);
            if(MapData.Save(Path(m_MapName), mapData))
            {
                Debug.Log("Saved Map");
            }
        }

        private void LoadMap()
        {
            MapData loadedMapData;
            if (MapData.Load(Path(m_MapName), out loadedMapData))
            {
                m_RawMapData = new HashSet<Hex>(loadedMapData);
                Debug.Log("Loaded Map");
                foreach (Hex hex in loadedMapData)
                {
                    CreateTileAt(hex);
                }
            }
        }

        private void CreateTileAt(Hex hex)
        {
            Tile tile = Instantiate(m_TileObject, this.transform);
            tile.transform.position = (Vector3)m_Layout.HexToPixel(hex) + (Vector3.back * height * 0.2f);
            tile.hideFlags = HideFlags.HideAndDontSave;
            m_Tiles.Add(hex, tile);
        }

        private void LoadOrCreateMap()
        {
            if (!File.Exists(Path(m_MapName)))
            {
                m_RawMapData = new HashSet<Hex>();
                SaveMap();
            }
            LoadMap();
        }

        private void Update()
        {
            if (m_RawMapData == null)
            {
                return;
            }

            height = Mathf.Clamp(height + (int)Input.mouseScrollDelta.y, 0, 3);
            Plane checkPlane = new Plane(Vector3.back, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (checkPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Hex hitHex = m_Layout.PixelToHex((Vector2)hitPoint).HexRound();
                m_TilePreview.transform.position = (Vector3)m_Layout.HexToPixel(hitHex) + (Vector3.back * height * 0.2f);
                m_MouseHex = hitHex;

                m_PreviewRenderer.material.color = m_RawMapData.Contains(hitHex) ? Color.red : Color.green;

                if (Input.GetMouseButton(0))
                {
                    if (m_RawMapData.Add(hitHex))
                    {
                        CreateTileAt(hitHex);
                    }
                }
                if (Input.GetMouseButton(1))
                {
                    if (m_RawMapData.Remove(hitHex))
                    {
                        Destroy(m_Tiles[hitHex].gameObject);
                        m_Tiles.Remove(hitHex);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveMap();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadOrCreateMap();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Draw();
        }

        private void Draw()
        {
            if (m_RawMapData != null)
            {
                Color drawColor = Color.white;
                foreach (Hex hex in m_RawMapData)
                {
                    List<Vector2> points = m_Layout.PolygonCorners(hex, m_DrawScale);
                    List<Vector2> points2 = m_Layout.PolygonCorners(hex, m_DrawScale * 0.95f);
                    Gizmos.color = drawColor;
                    for (int i = 0; i < 6; i++)
                    {
                        Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
                        Gizmos.DrawLine(points2[i], points2[(i + 1) % 6]);
                    }
                }

                Gizmos.color = m_RawMapData.Contains(m_MouseHex) ? Color.red : Color.green;
                List<Vector2> mousePoints = m_Layout.PolygonCorners(m_MouseHex, m_DrawScale);
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(mousePoints[i], mousePoints[(i + 1) % 6]);
                }
            }
        }
    }
}