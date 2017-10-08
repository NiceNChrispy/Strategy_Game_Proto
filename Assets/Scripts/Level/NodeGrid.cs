using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    private Node<Vector3>[,] m_TileNodes;
    [SerializeField] int m_Width, m_Height;

    private Node<Vector3> this[int x, int y]
    {
        get
        {
            return (x >= 0 && x < m_Width && y >= 0 && y < m_Height) ? m_TileNodes[x, y] : null;
        }

        set
        {
            m_TileNodes[x, y] = value;
        }
    }

    private void Start()
    {
        Create();
        Connect();
    }

    public void Create()
    {
        m_TileNodes = new Node<Vector3>[m_Width, m_Height];

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                m_TileNodes[x, y] = new Node<Vector3>(4);
                m_TileNodes[x, y].Data = new Vector3(x, 1, y);
            }
        }
    }

    public void Connect()
    {
        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                int i = 0;

                m_TileNodes[x, y][i++] = this[x,     y + 1];
                m_TileNodes[x, y][i++] = this[x + 1, y    ];
                m_TileNodes[x, y][i++] = this[x,     y - 1];
                m_TileNodes[x, y][i++] = this[x - 1, y    ];
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Node<Vector3> node = m_TileNodes[x, y][i];

                        if(node != null)
                        {
                            Gizmos.DrawRay(m_TileNodes[x, y].Data, (node.Data - m_TileNodes[x, y].Data).normalized * 0.5f);
                        }
                    }
                }
            }
        }
    }
}