using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2 : MonoBehaviour
{
    public static event Action<Unit2, bool> OnHighlight = delegate { };
    public static event Action<Unit2, bool> OnSelect = delegate { };

    private void OnMouseEnter()
    {
        OnHighlight(this, true);
    }

    private void OnMouseExit()
    {
        OnHighlight(this, false);
    }

    private void OnMouseDown()
    {
        OnSelect(this, true);
    }

    private void OnMouseUp()
    {
        OnSelect(this, true);
    }
}