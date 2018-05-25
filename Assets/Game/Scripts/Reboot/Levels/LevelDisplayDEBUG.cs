using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class LevelDisplayDEBUG : MonoBehaviour
    {
        [SerializeField] private LevelData m_LevelData;

        void OnDrawGizmos()
        {
            if (m_LevelData != null)
            {
                foreach (Tile tile in m_LevelData.Tiles)
                {
                    List<Vector2> points = m_LevelData.Layout.PolygonCorners(tile.Position);
                    for (int i = 0; i < 6; i++)
                    {
                        Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
                    }
                }
            }
        }
    }
}