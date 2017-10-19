using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    [SerializeField] private HexTile m_HexTilePrefab;
    [SerializeField] private int m_Rows, m_Columns;
    [SerializeField] private Vector2 m_Dimensions;
    [SerializeField] private Vector2 m_RelativeOffset;
    [SerializeField] private float levelGenSpeed;

    HexTile[,] spawnedTiles;

    private void Start()
    {
        spawnedTiles = new HexTile[m_Rows, m_Columns];
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        for (int y = 0; y < m_Columns; y++)
        {
            for (int x = 0; x < m_Rows; x++)
            {
                HexTile spawnedTile = Instantiate(m_HexTilePrefab, HexPosFromGrid(x, y), Quaternion.identity);
                spawnedTile.SetPosition(y, x);
                spawnedTile.transform.parent = transform;
                spawnedTiles[x, y] = spawnedTile;
                yield return new WaitForSeconds(levelGenSpeed);
            }
        }
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

    Vector3 HexPosFromGrid(int row, int col)
    {
        float x = (col * 0.866f) + (0.433f * (row & 1));
        float z = row * 0.75f;

        return new Vector3(x, 0, z);
    }

    Vector2Int HexPosFromGrid(Vector3Int position)
    {
        int col = position.x + (position.z - (position.z & 1)) / 2;
        int row = position.z;
        return new Vector2Int(col, row);
    }
}