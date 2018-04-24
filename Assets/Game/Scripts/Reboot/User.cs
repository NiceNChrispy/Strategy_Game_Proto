using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class User : MonoBehaviour
    {
        [SerializeField] private Selector m_Selector;
        [SerializeField] private Camera m_Camera;

        public void Select()
        {
            Vector3 cursorPosition;
            cursorPosition.x = Input.mousePosition.x;
            cursorPosition.y = Input.mousePosition.y;
            cursorPosition.z = m_Camera.nearClipPlane;

            Ray screenRay = m_Camera.ScreenPointToRay(cursorPosition);
            m_Selector.Select(screenRay.origin, screenRay.direction);
        }
    }
}