using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    [ExecuteInEditMode]
    public class LevelEditor : MonoBehaviour
    {
        public Level m_LevelBeingEdited;

        [SerializeField] private Tile loadedTile;

        private void OnDrawGizmos()
        {
            if(!m_LevelBeingEdited)
            {
                return;
            }

            List<Vector2> points = new List<Vector2>();
            foreach(Tile tile in m_LevelBeingEdited.Tiles)
            {
                points = m_LevelBeingEdited.Layout.PolygonCorners(tile.Position);

                for (int i = 0; i < points.Count; i++)
                {
                    Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
                }
            }
        }

        [NaughtyAttributes.Button]
        public void Load()
        {
            loadedTile = Resources.Load<Tile>("HexTile");
        }
    }
}