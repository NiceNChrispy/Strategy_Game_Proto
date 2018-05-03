using System;
using UnityEngine;

namespace Reboot
{
    public class Selector<MonoBehaviour>
    {
        LayerMask m_SelectionLayer;
        public Selector(LayerMask selectionLayer)
        {
            m_SelectionLayer = selectionLayer;
        }

        public ISelectableComponent<MonoBehaviour> CurrentSelectable
        {
            get; set;
        }

        private Collider m_PreviousCollider;

        public void UpdateSelection(Ray cursorRay)
        {
            RaycastHit hit;

            if (Physics.Raycast(cursorRay, out hit, Mathf.Infinity, m_SelectionLayer))
            {
                //Debug.DrawLine(cursorRay.direction, hit.point, Color.green);

                if (hit.collider != m_PreviousCollider)
                {
                    DeselectCurrent();
                    CurrentSelectable = hit.collider.GetComponent<ISelectableComponent<MonoBehaviour>>();
                    if (CurrentSelectable != null && CurrentSelectable.IsSelectable)
                    {
                        CurrentSelectable.OnCursorEnter();
                    }
                    m_PreviousCollider = hit.collider;
                }
            }
            else
            {
                DeselectCurrent();
                m_PreviousCollider = null;
                Debug.DrawRay(cursorRay.origin, cursorRay.direction * Mathf.Infinity, Color.red);
            }
        }

        public void DeselectCurrent()
        {
            if (CurrentSelectable != null)
            {
                //SelectableUnderCursor.Deselect();
                CurrentSelectable.OnCursorExit();
                CurrentSelectable = null;
            }
        }
    }
}