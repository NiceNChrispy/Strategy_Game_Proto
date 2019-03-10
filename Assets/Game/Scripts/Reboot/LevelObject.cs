using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class LevelObject : MonoBehaviour
    {
        [SerializeField] private Level m_Level;

        void OnDrawGizmos()
        {
            //foreach(INavNode<Hex> tile in m_Level.Tiles.Nodes)
            //{
            //    foreach(INavNode<Hex> connected in tile.Connected)
            //    {
            //        Gizmos.DrawLine(Layout.Default.HexToPixel(tile.Position), Layout.Default.HexToPixel(connected.Position));
            //    }
            //}
        }
    }
}
