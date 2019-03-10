using System.Collections.Generic;
using UnityEngine;

namespace SecondAttempt
{
    public class Level : MonoBehaviour
    {
        Dictionary<Hex, Tile> m_HexTileLookup;

        public void OnEnable()
        {
            m_HexTileLookup = new Dictionary<Hex, Tile>();
        }

        public void CreateTileAt(Vector2 worldPosition)
        {
            Tile tileAtPosition;
            if (!TryGetTile(worldPosition, out tileAtPosition))
            {
                Hex hexPosition = WorldPositionToHex(worldPosition);
                m_HexTileLookup.Add(hexPosition, new Tile(hexPosition));
            }
        }

        public void RemoveTileAt(Vector2 worldPosition)
        {
            Tile tileAtPosition;
            if (TryGetTile(worldPosition, out tileAtPosition))
            {
                Hex hexPosition = WorldPositionToHex(worldPosition);
                m_HexTileLookup.Remove(hexPosition);
            }
        }

        public bool TryGetTile(Vector2 position, out Tile tileAtPosition)
        {
            return m_HexTileLookup.TryGetValue(WorldPositionToHex(position), out tileAtPosition);
        }

        public Vector2 HexPositionToWorld(Hex hex)
        {
            return transform.TransformPoint(Layout.Default.HexToPixel(hex));
        }


        public Hex WorldPositionToHex(Vector2 worldPosition)
        {
            return Layout.Default.PixelToHex(worldPosition).HexRound();
        }

        public Vector2 SnapWorldPositionToHex(Vector2 worldPosition)
        {
            return HexPositionToWorld(WorldPositionToHex(worldPosition));
        }

        public void OnDrawGizmosSelected()
        {
            if (m_HexTileLookup != null)
            {
                foreach (Hex hex in m_HexTileLookup.Keys)
                {
                    Gizmos.DrawSphere(HexPositionToWorld(hex), 0.2f);
                }
            }
        }
    }
}