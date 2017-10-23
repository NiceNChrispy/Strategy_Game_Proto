using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectableObject : MonoBehaviour
{
    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;
    public UnityEvent OnTarget;
    public UnityEvent OnUnTarget;

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