using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    private Node<T>[] m_ConnectedNodes;
    private T m_Data;

    public Node<T>[] ConnectedNodes
    {
        get
        {
            return m_ConnectedNodes;
        }
    }
    public T Data
    {
        get
        {
            return m_Data;
        }

        set
        {
            m_Data = value;
        }
    }

    public Node(int connectionCount)
    {
        m_ConnectedNodes = new Node<T>[connectionCount];
    }

    public Node<T> this[int index]
    {
        get
        {
            return m_ConnectedNodes[index];
        }

        set
        {
            m_ConnectedNodes[index] = value;
        }
    }
}