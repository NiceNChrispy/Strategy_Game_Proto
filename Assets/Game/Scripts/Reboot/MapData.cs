using DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map", order = 1)]
    public class MapData : ScriptableObject
    {
        NavGraph<HexTile> m_NavGraph;

        bool Contains(HexTile hexTile)
        {
            return m_NavGraph.Contains(hexTile);
        }

        public IEnumerable<HexTile> GetPath(HexTile from, HexTile to)
        {
            List<AStarNode<HexTile>> nodePath = m_NavGraph.GetPath(from, to, (x, y) => x.Position.Distance(y.Position));

            return nodePath.Select(x => x.Data);
        }

        public IEnumerable<HexTile> GetHexesInRange(HexTile tile, int range)
        {
            List<AStarNode<HexTile>> nodesInRange = m_NavGraph.GetNodesInRange(tile, range);
            return nodesInRange.Select(x => x.Data);
        }
    }
}