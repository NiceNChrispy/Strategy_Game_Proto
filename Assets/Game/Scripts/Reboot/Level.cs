using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    [CreateAssetMenu(fileName = "Level", menuName = "Reboot/Level", order = 1)]
    public class Level : ScriptableObject
    {
        [NaughtyAttributes.ReorderableList]
        public List<Tile> Tiles = new List<Tile>();

        private Layout m_Layout = new Layout(Layout.POINTY, Vector2.one, Vector2.zero);
        public Layout Layout
        {
            get
            {
                return m_Layout;
            }
        }
    }
}