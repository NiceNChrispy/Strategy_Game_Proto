using System.Collections.Generic;

namespace Reboot
{
    [System.Serializable]
    public class Map
    {
        public List<Hex> Tiles;

        public Map()
        {
            Tiles = new List<Hex>();
        }

        public void Add(Hex hex)
        {
            if(!Tiles.Contains(hex))
            {
                Tiles.Add(hex);
            }
        }

        public void Remove(Hex hex)
        {
            if (Tiles.Contains(hex))
            {
                Tiles.Remove(hex);
            }
        }

        public bool Contains(Hex hex)
        {
            return Tiles.Contains(hex);
            //return Tiles.Any(x => x.Position == hex);
        }
    }

    public enum TileType
    {
        NORMAL, WATER
    }

    [System.Serializable]
    public class HexTileData
    {
        public Hex Position = new Hex();
        public float Height = 0f;
        public TileType Type = TileType.NORMAL;
        public bool IsOccupied = false;

        public HexTileData(Hex position, float height)
        {
            Position = position;
            Height = height;
        }
    }
}