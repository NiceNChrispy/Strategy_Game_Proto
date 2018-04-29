using DataStructures;
using UnityEngine;

namespace Reboot
{
    public class HexTile : MonoBehaviour
    {
        public AStarNode<Hex> Node { get; set; }
        public Hex Position { get { return Node.Data; } }

        public void Init(Hex position)
        {
            Node.Data = position;
        }
    }
}
