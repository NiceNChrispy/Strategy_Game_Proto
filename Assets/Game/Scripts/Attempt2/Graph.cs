using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class Node2<T>
    {
        T Data;
        public List<Node2<T>> Connections;

        public Node2(T data)
        {
            Data = data;
            Connections = new List<Node2<T>>();
        }
    }
}