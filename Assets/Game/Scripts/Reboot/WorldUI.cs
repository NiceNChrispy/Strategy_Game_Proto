using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private Renderer m_TileMarker;

        [SerializeField] private Color m_DefaultColor;
        [SerializeField] private Color m_MoveColor;
        [SerializeField] private Color m_AttackColor;

        Dictionary<Tile, Renderer> m_TileMarkers;

        private void Start()
        {
            GameManager.Instance.OnGameBegin += InitMarkers;
        }

        private void InitMarkers()
        {
            m_TileMarkers = new Dictionary<Tile, Renderer>();

            foreach(Tile tile in GameManager.Instance.Tiles)
            {
                Renderer tileMarker = Instantiate(m_TileMarker);
                tileMarker.transform.parent = this.transform;
                tileMarker.transform.position = tile.transform.position + (Vector3.back * 0.00001f);
                m_TileMarkers.Add(tile, tileMarker);
            }
        }

        private void OnEnable()
        {
            GameManager.Instance.OnPlayerUpdateTiles += OnTilesUpdated;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPlayerUpdateTiles += OnTilesUpdated;
        }

        private void OnTilesUpdated()
        {
            foreach(Tile tile in GameManager.Instance.Tiles)
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
                foreach (Tile tile in GameManager.Instance.PlayerWithTurn.MoveableTiles)
                {
                    m_TileMarkers[tile].material.color = m_MoveColor;
                    Vector3 tilePos = tile.transform.position;
                    tilePos.z -= 0.0002f;
                    m_TileMarkers[tile].transform.position = tilePos;
                }
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
    }
}