using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    [CreateAssetMenu(fileName = "Level", menuName = "Reboot/Level", order = 1)]
    public class LevelData : ScriptableObject
    {
        [NaughtyAttributes.ReorderableList]
        public List<Tile> Tiles = new List<Tile>();

        private Layout m_Layout = new Layout(Layout.FLAT, Vector2.one, Vector2.zero);
        public Layout Layout
        {
            get
            {
                return m_Layout;
            }
            set
            {
                m_Layout = value;
            }
        }
    }
}