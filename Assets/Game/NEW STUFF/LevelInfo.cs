using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondAttempt
{
    public struct LevelInfo
    {
        public string Name;
        public int NumPlayers;
        public List<Tile> Tiles;
    }

    public struct PlayerInfo
    {
        public string Name;
    }
}