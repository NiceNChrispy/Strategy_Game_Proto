using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu(menuName = "Pattern/Grid")]
    public class GridPattern : Pattern
    {
        [SerializeField] public Vector2Int m_Size;

        public override Vector2Int Size
        {
            get
            {
                return m_Size;
            }
        }

        public override List<Vector2Int> GetPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int y = 0; y < m_Size.y; y++)
            {
                for (int x = 0; x < m_Size.x; x++)
                {
                    positions.Add(new Vector2Int(x, y));
                }
            }

            return positions;
        }
    }
}