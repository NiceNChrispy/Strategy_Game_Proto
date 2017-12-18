using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}