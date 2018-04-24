using UnityEngine;

namespace Reboot
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private LayerMask m_SelectionLayer;
        [SerializeField] private Camera m_Camera;

        public ISelectable m_SelectableUnderCursor
        {
            get; set;
        }
        public ISelectable CurrentSelectable
        {
            get; set;
        }

        private Collider m_PreviousCollider;

        public void Select(Vector3 position, Vector3 direction)
        {
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, Mathf.Infinity, m_SelectionLayer))
            {
                Debug.DrawLine(position, hit.point, Color.green);

                if (hit.collider != m_PreviousCollider)
                {
                    DeselectCurrent();
                    m_SelectableUnderCursor = hit.collider.GetComponent<ISelectable>();
                    if(m_SelectableUnderCursor != null && m_SelectableUnderCursor.IsSelectable)
                    {
                        m_SelectableUnderCursor.Select();
                    }
                }
                m_PreviousCollider = hit.collider;
            }
            else
            {
                DeselectCurrent();
                m_PreviousCollider = null;
                Debug.DrawRay(position, direction * Mathf.Infinity, Color.red);
            }
        }

        public void DeselectCurrent()
        {
            if (m_SelectableUnderCursor != null)
            {
                m_SelectableUnderCursor.Deselect();
                m_SelectableUnderCursor = null;
            }
        }
    }
}