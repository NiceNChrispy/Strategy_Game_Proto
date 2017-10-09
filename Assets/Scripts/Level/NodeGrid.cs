using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    private Node<Vector3>[,] m_TileNodes;
    [SerializeField] int m_Width, m_Height;

    Node<Vector3>[] m_Accessible;

    [SerializeField] private int xPos, yPos;
    [SerializeField] private float minRange, maxRange;
    [SerializeField] private bool isManhatten;

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

    private void Update()
    {
        m_Accessible = GetAccessable(xPos, yPos, minRange, maxRange, isManhatten);
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

            Gizmos.color = Color.red;

            for (int i = 0; i < m_Accessible.Length; i++)
            {
                Gizmos.DrawSphere(m_Accessible[i].Data, 0.2f);
            }

        }
    }

    public Node<Vector3>[] GetAccessable(int x, int y, int min, int max)
    {
        List<Node<Vector3>> accessible = new List<Node<Vector3>>();
        for (int iy = 0; iy < m_Height; iy++)
        {
            for (int ix = 0; ix < m_Width; ix++)
            {
                int distance = Mathf.Abs(ix - x) + Mathf.Abs(iy - y);
                if(distance >= min && distance <= max)
                {
                    accessible.Add(this[ix, iy]);
                }
            }
        }
        return accessible.ToArray();
    }

    public Node<Vector3>[] GetAccessable(int x, int y, float min, float max, bool manhatten = true)
    {
        List<Node<Vector3>> accessible = new List<Node<Vector3>>();
        for (int iy = 0; iy < m_Height; iy++)
        {
            for (int ix = 0; ix < m_Width; ix++)
            {
                float distance = manhatten ? Mathf.Abs(ix - x) + Mathf.Abs(iy - y) : Vector2.Distance(new Vector2(x,y), new Vector2(ix,iy));
                if (distance >= min && distance <= max)
                {
                    accessible.Add(this[ix, iy]);
                }
            }
        }
        return accessible.ToArray();
    }
}