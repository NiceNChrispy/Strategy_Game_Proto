using System;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    public event Action OnTarget = delegate { };
    public event Action OnUnTarget = delegate { };

    public void Target()
    {
        //Debug.Log("Target " + name);
        OnTarget.Invoke();
    }

    public void UnTarget()
    {
        //Debug.Log("UnTargeted " + name);
        OnUnTarget.Invoke();
    }

    public void Select()
    {
        //Debug.Log("Selected " + name);
        OnSelect.Invoke();
    }

    public void Deselect()
    {
        //Debug.Log("Deselected " + name);
        OnDeselect.Invoke();
    }
}