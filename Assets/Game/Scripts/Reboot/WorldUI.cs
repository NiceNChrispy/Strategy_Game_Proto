using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private Renderer m_TileMarker;

        [SerializeField] private Color m_DefaultColor;
        [SerializeField] private Color m_MoveColor;
        [SerializeField] private Color m_AttackColor;

        [SerializeField] private GameManager m_GameManager;

        Dictionary<Tile, Renderer> m_TileMarkers;

        List<Vector2> border = new List<Vector2>();

        private void Start()
        {
            m_GameManager.OnGameBegin += CreateMarkers;
        }

        private void CreateMarkers()
        {
            m_TileMarkers = new Dictionary<Tile, Renderer>();

            foreach(Tile tile in m_GameManager.Tiles)
            {
                Renderer tileMarker = Instantiate(m_TileMarker);
                tileMarker.transform.parent = this.transform;
                tileMarker.transform.position = tile.transform.position + (Vector3.back * 0.001f);
                m_TileMarkers.Add(tile, tileMarker);
            }
        }

        private void OnEnable()
        {
            m_GameManager.OnPlayerUpdateTiles += OnTilesUpdated;
        }

        private void OnDisable()
        {
            m_GameManager.OnPlayerUpdateTiles -= OnTilesUpdated;
        }

        private void Update()
        {
            for (int i = 0; i < border.Count - 1; i += 2)
            {
                Debug.DrawLine((Vector3)border[i] + Vector3.back, (Vector3)border[i + 1] + Vector3.back);
            }
        }

        private void OnTilesUpdated()
        {
            foreach(Tile tile in m_GameManager.Tiles)
            {
                m_TileMarkers[tile].material.color = m_DefaultColor;
                Vector3 tilePos = tile.transform.position;
                tilePos.z -= 0.0001f;
                m_TileMarkers[tile].transform.position = tilePos;
            }
            switch (GameManager.Instance.PlayerWithTurn.CurrentOrder)
            {
                case OrderType.NONE:
                break;
                case OrderType.MOVE:
                foreach (Tile tile in m_GameManager.PlayerWithTurn.MoveableTiles)
                {
                    m_TileMarkers[tile].material.color = m_MoveColor;
                    Vector3 tilePos = tile.transform.position;
                    tilePos.z -= 0.0002f;
                    m_TileMarkers[tile].transform.position = tilePos;
                }

                border = GetBorder(m_GameManager.PlayerWithTurn.MoveableTiles);
                break;
                case OrderType.ATTACK:
                foreach (Tile tile in GameManager.Instance.PlayerWithTurn.AttackableTiles)
                {
                    m_TileMarkers[tile].material.color = m_AttackColor;
                    Vector3 tilePos = tile.transform.position;
                    tilePos.z -= 0.0002f;
                    m_TileMarkers[tile].transform.position = tilePos;
                }
                break;
            }
        }

        List<Vector2> GetBorder(List<Tile> tiles)
        {
            List<Hex> tilePositions = tiles.Select(x => x.Position).ToList();
            Queue<Hex> openSet = new Queue<Hex>(tilePositions);
            List<Hex> closedSet = new List<Hex>();
            List<Vector2> borderPositions = new List<Vector2>();

            while (openSet.Count > 0)
            {
                Hex currentHex = openSet.Dequeue();
                closedSet.Add(currentHex);

                for (int i = 0; i < 6; i++)
                {
                    if(closedSet.Contains(currentHex.Neighbor(i)))
                    {
                        continue;
                    }
                    if(!tilePositions.Contains(currentHex.Neighbor(i)))
                    {
                        Vector2 hexPosition = Layout.Default.HexToPixel(currentHex);
                        borderPositions.Add(hexPosition + Layout.Default.HexCornerOffset((i - 1) % 6));
                        borderPositions.Add(hexPosition + Layout.Default.HexCornerOffset(i));
                    }
                }
            }
            return borderPositions;
        }
    }
}