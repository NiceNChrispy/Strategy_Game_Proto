using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    [SerializeField] private float m_Range = 2.5f;
    [SerializeField] private LayerMask m_Layer;
    [SerializeField] private Camera m_Camera;

    [Space(5), ReadOnly, SerializeField] private string m_Current;
    [ReadOnly, SerializeField]           private string m_Selected;

    public SelectableObject CurrentObject
    {
        get; set;
    }
    public SelectableObject SelectedObject
    {
        get; set;
    }

    private Collider m_PreviousCollider;

    public void Update()
    {
        RaycastHit hit;
        Vector3 direction = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)) - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, m_Range, m_Layer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);

            if (hit.collider != m_PreviousCollider)
            {
                ClearCurrent();
                CurrentObject = hit.collider.GetComponent<SelectableObject>();
                CurrentObject.Target();
            }
            m_PreviousCollider = hit.collider;
        }
        else
        {
            ClearCurrent();
            m_PreviousCollider = null;
            Debug.DrawRay(transform.position, direction * m_Range, Color.red);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (SelectedObject != null)
            {
                SelectedObject.Deselect();
                SelectedObject = null;
            }
            if (CurrentObject)
            {
                SelectedObject = CurrentObject;
                SelectedObject.Select();
            }
        }
        m_Selected = SelectedObject ? SelectedObject.name : "None";
        m_Current = CurrentObject ? CurrentObject.name : "None";
    }

    private void ClearCurrent()
    {
        if (CurrentObject != null)
        {
            CurrentObject.UnTarget();
            CurrentObject = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SelectedObject != null)
        {
            SelectedObject.Deselect();
            SelectedObject = null;
        }
        if (CurrentObject)
        {
            SelectedObject = CurrentObject;
            SelectedObject.Select();
        }
    }
}