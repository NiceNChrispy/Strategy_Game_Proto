using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Prototype
{
    public static class HexUtil
    {
        public static Vector3 WorldPosFromHexPos(Vector2Int hexPosition)
        {
            float xPos = (hexPosition.x * 0.866f) + (0.433f * (hexPosition.y & 1));
            float yPos = hexPosition.y * 0.75f;

            return new Vector3(xPos, yPos, 0);
        }

        public static Vector2Int CubeToHex(Vector3Int cubePos)
        {
            Vector2Int hexPos = new Vector2Int();
            hexPos.x = cubePos.x + (cubePos.z - (cubePos.z & 1)) / 2;
            hexPos.y = cubePos.z;
            return hexPos;
        }

        public static Vector3Int HexToCube(Vector2Int hexPosition)
        {
            Vector3Int cubePos = new Vector3Int();
            cubePos.x = hexPosition.x - (hexPosition.y - (hexPosition.y & 1)) / 2;
            cubePos.z = hexPosition.y;
            cubePos.y = -cubePos.x - cubePos.z;
            return cubePos;
        }

        public static float Distance(Vector3Int from, Vector3Int to)
        {
            return Mathf.Max(Mathf.Abs(from.x - to.x), Mathf.Abs(from.y - to.y), Mathf.Abs(from.z - to.z));
        }

        public static float Distance(Vector2Int from, Vector2Int to)
        {
            return Distance(HexToCube(from), HexToCube(to));
        }

        public static List<Vector2Int> CoordsInRange(Vector2Int position, int range)
        {
            List<Vector2Int> coordsInRange = new List<Vector2Int>();

            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
                {
                    coordsInRange.Add(position + CubeToHex(new Vector3Int(dx, dy, -dx - dy)));
                }
            }

            return coordsInRange;
        }
    }
}