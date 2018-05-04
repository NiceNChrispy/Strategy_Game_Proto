using DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace Reboot
{
    public class GameBoard
    {
        Dictionary<Hex, Tile> m_Tiles;

        public List<Tile> Tiles
        {
            get
            {
                return m_Tiles.Values.ToList();
            }
        }

        public void AddTileAt(Hex position, Tile tile)
        {
            if (!m_Tiles.ContainsKey(position))
            {
                m_Tiles.Add(position, tile);
            }
        }

        public void RemoveTileAt(Hex position)
        {
            m_Tiles.Remove(position);
        }

        public void GetPath()
        {

        }
    }
}