using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private User m_User;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_User.Select();
            }
        }
    }
}
