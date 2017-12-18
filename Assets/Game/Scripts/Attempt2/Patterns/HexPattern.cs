using UnityEngine;
using System.Collections.Generic;

namespace Prototype
{
    [CreateAssetMenu(menuName = "Pattern/HexPattern")]
    public class HexPattern : Pattern
    {
        public int RingCount;

        public override Vector2Int Size
        {
            get
            {
                return new Vector2Int(((RingCount - 1) * 2) + 1, ((RingCount - 1) * 2) +1);
            }
        }

        public override List<Vector2Int> GetPositions()
        {
            return HexUtil.CoordsInRange(Vector2Int.zero, RingCount - 1);
        }
    }
}
