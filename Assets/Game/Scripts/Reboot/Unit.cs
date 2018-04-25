using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectable
    {
        public bool IsSelectable
        {
            get; set;
        }
        
        private void OnEnable()
        {
            IsSelectable = true;
        }

        public void Select()
        {
            Debug.Log("Selected");
        }

        public void Deselect()
        {
            Debug.Log("Deselected");
        }


    }
}