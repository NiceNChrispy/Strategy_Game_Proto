using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class HexSpawner : MonoBehaviour
{
    [SerializeField] private HexTile m_HexTilePrefabNormal, m_HexTilePrefabOther;
    [SerializeField] private int m_Columns, m_Rows;
    [SerializeField] private UnityEngine.Vector2 m_Dimensions;
    [SerializeField] private UnityEngine.Vector2 m_RelativeOffset;
    [SerializeField] private float levelGenSpeed;

    HexTile[,] spawnedTiles;

    public LevelLoader mapLoader;

    private void Start()
    {

        spawnedTiles = new HexTile[m_Columns, m_Rows];
        //StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        HexTile spawnedTile;
        for (int i = 0; i < mapLoader.mapData.Length; i += 0)
        {
            for (int y = 0; y < m_Rows; y++)
            {
                for (int x = 0; x < m_Columns; x++)
                {
                    switch (mapLoader.mapData[i])
                    {
                        case "0":
                            spawnedTile = Instantiate(m_HexTilePrefabNormal, HexPosFromGrid(x, y), Quaternion.identity);
                            break;

                        case "1":
                             spawnedTile = Instantiate(m_HexTilePrefabOther, HexPosFromGrid(x, y), Quaternion.identity);
                            break;

                        default:
                             spawnedTile = Instantiate(m_HexTilePrefabNormal, HexPosFromGrid(x, y), Quaternion.identity);
                            break;
                    }

                    //HexTile spawnedTile = Instantiate(m_HexTilePrefabNormal, HexPosFromGrid(x, y), Quaternion.identity);
                    spawnedTile.SetPosition(x, y);
                    spawnedTile.transform.parent = transform;
                    spawnedTiles[x, y] = spawnedTile;
                    yield return new WaitForSeconds(levelGenSpeed);
                    i++;
                }
            }
        }
    }

    public void MapSpawn()
    {
        StartCoroutine(Spawn());
    }
  

    //void Update()
    //{
    //    for (int y = 0; y < m_Columns; y++)
    //    {
    //        for (int x = 0; x < m_Rows; x++)
    //        {
    //            if (spawnedTiles[x, y])
    //            {
    //                spawnedTiles[x, y].transform.localPosition = HexPosFromGrid(x, y);
    //            }
    //        }
    //    }
    //}

    //Vector3 HexPosFromGrid(int row, int col)
    //{
    //    Vector3 pos;
    //    pos.x = (col * m_Dimensions.x * m_RelativeOffset.x) + ((row & 1) * 0.5f * m_Dimensions.x);
    //    pos.y = 0;
    //    pos.z = (row * m_Dimensions.y * m_RelativeOffset.y);
    //    return pos;
    //}

    public static Vector3 HexPosFromGrid(int row, int col)
    {
        float xPos = (row * 0.866f) + (0.433f * (col & 1));
        float zPos = col * 0.75f;

        return new Vector3(xPos, 0, zPos);
    }

    //Vector2Int HexPosFromGrid(Vector3Int position)
    //{
    //    int col = position.x + (position.z - (position.z & 1)) / 2;
    //    int row = position.z;
    //    return new Vector2Int(col, row);
    //}
}