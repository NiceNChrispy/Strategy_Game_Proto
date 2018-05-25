using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Map<Hex> m_Map = new Map<Hex>();

        public List<Hex> Contents { get { return m_Map.Contents; } set { m_Map.Contents = value; } } 

        public bool Contains(Hex hex)
        {
            return m_Map.Contains(hex);
        }

        public void Add(Hex hex)
        {
            m_Map.Add(hex);
        }

        public void Remove(Hex hex)
        {
            m_Map.Remove(hex);
        }
    }
}