using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;
    public UnityEvent OnLeftMouseDown;
    public UnityEvent OnLeftMouseUp;
    public UnityEvent OnRightMouseDown;
    public UnityEvent OnRightMouseUp;

    public void MouseEnter()
    {
        OnMouseEnter.Invoke();
        //print("Mouse Enter");
    }

    public void MouseExit()
    {
        OnMouseExit.Invoke();
        //print("Mouse Exit");
    }

    public void LeftMouseDown()
    {
        OnLeftMouseDown.Invoke();
        //print("Left Mouse Down");
    }

    public void LeftMouseUp()
    {
        OnLeftMouseUp.Invoke();
        //print("Left Mouse Up");
    }

    public void RightMouseDown()
    {
        OnRightMouseDown.Invoke();
        //print("Right Mouse Down");
    }

    public void RightMouseUp()
    {
        OnRightMouseUp.Invoke();
        //print("Right Mouse Up");
    }
}