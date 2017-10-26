using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class NodeBehaviour : MonoBehaviour
    {
        Node<TestClass> m_Node;
    }

    [System.Serializable]
    public class TestClass : IComparable<TestClass>
    {
        public int CompareTo(TestClass other)
        {
            throw new NotImplementedException();
        }
    }
}