using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public Hex HexPosition;
    public Vector3 WorldPosition;

    public TileData(Hex hexPosition, Vector3 worldPosition)
    {
        HexPosition = hexPosition;
        WorldPosition = worldPosition;
    }
}