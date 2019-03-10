using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondAttempt
{
    public struct Tile
    {
        public Hex Position;

        public Tile(Hex position)
        {
            Position = position;
        }
    }
}
