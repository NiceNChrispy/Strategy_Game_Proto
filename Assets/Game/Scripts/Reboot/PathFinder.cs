using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public abstract class PathFinder<T>
    {
        protected Graph<T> m_Graph;

        public PathFinder(Graph<T> graph)
        {
            m_Graph = graph;
        }

        public abstract List<Vertex<T>> GetPath(Vertex<T> from, Vertex<T> to, Func<T, T, float> distanceFunction);
    }
}