using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reboot
{
    public interface INavigationNode<T>
    {
        T Data { get; set; }
        IEnumerable<INavigationNode<T>> Connections { get; set; }
    }

    //public class NavigationGrid
    //{
    //    IEnumerable<INavigationNode<T>> m_Nodes;
    //    INavigationNode[][] m_Edges;

    //    public NavigationGrid(IEnumerable<INavigationNode> nodes)
    //    {
    //        m_Nodes = nodes;
    //    }

    //    public void Construct()
    //    {
    //        m_Edges = new INavigationNode[m_Nodes.Count()][];

    //        for (int i = 0; i < m_Nodes.Count(); i++)
    //        {
    //            IEnumerable<INavigationNode> connections = m_Nodes.ElementAt(i).Connections;
    //            if(connections != null)
    //            {
    //                m_Edges[i] = connections.ToArray();
    //            }
    //        }
    //    }

    //    public void Draw()
    //    {
    //        for (int i = 0; i < m_Edges.Length; i++)
    //        {
    //            for (int j = 0; j < m_Edges[i].Length; j++)
    //            {
    //                Debug.DrawLine(m_Edges[i].position, m_Edges[j])
    //            }
    //        }
    //    }
    //}
}