using UnityEngine;

namespace Reboot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] LevelData m_LevelData;
        [SerializeField] private Tile m_TilePrefab;

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
    }
}