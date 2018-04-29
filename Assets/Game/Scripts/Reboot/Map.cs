using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Reboot
{
    [System.Serializable]
    public class Map
    {
        public List<Hex> Hexes;

        public Map()
        {
            Hexes = new List<Hex>();
        }

        public void Add(Hex hex)
        {
            if(!Hexes.Contains(hex))
            {
                Hexes.Add(hex);
            }
        }

        public void Remove(Hex hex)
        {
            if (Hexes.Contains(hex))
            {
                Hexes.Remove(hex);
            }
        }

        public bool Contains(Hex hex)
        {
            return Hexes.Contains(hex);
        }
    }
}