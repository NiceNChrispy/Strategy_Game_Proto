using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : Singleton<Interactor>
{
    [SerializeField] private float m_Range = 2.5f;
    [SerializeField] private LayerMask m_Layer;
    [SerializeField] private Camera m_Camera;

    [Space(5), ReadOnly, SerializeField] private string m_Current;

    public Interactable Current
    {
        get; set;
    }

    private Collider m_PreviousCollider;

    private void Update()
    {
        UpdateSelection();   
    }

    public void UpdateSelection()
    {
        RaycastHit hit;
        Vector3 direction = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)) - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, m_Range, m_Layer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);

            if (hit.collider != m_PreviousCollider)
            {
                ClearCurrent();
                Current = hit.collider.GetComponent<Interactable>();
                if (Current)
                {
                    Current.MouseEnter();
                }
            }
            m_PreviousCollider = hit.collider;
        }
        else
        {
            ClearCurrent();
            m_PreviousCollider = null;
            Debug.DrawRay(transform.position, direction * m_Range, Color.red);
        }

        if (Input.GetMouseButtonDown(0) && Current)
        {
            Current.LeftMouseDown();
        }
        if (Input.GetMouseButtonUp(0) && Current)
        {
            Current.LeftMouseUp();
        }
        if (Input.GetMouseButtonDown(1) && Current)
        {
            Current.RightMouseDown();
        }
        if (Input.GetMouseButtonUp(1) && Current)
        {
            Current.RightMouseUp();
        }
        m_Current = Current ? Current.name : "None";
    }

    private void ClearCurrent()
    {
        if (Current != null)
        {
            Current.MouseExit();
            Current = null;
        }
    }
}