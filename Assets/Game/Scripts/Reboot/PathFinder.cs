using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public abstract class NavGraph<T> : Graph<T>
    {
        public abstract List<Vertex<T>> GetPath(Vertex<T> from, Vertex<T> to, Func<T, T, float> distanceFunction);
    }
}