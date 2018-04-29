using DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectableComponent<Unit>
    {
        [SerializeField, Range(0, 10)] private int m_MovementRange = 5;
        [SerializeField, Range(0, 10)] private int m_AttackRange = 3;

        public bool IsSelectable
        {
            get; set;
        }

        public int MovementRange
        {
            get
            {
                return m_MovementRange;
            }
        }
        public int AttackRange
        {
            get
            {
                return m_AttackRange;
            }
        }

        public Unit Component
        {
            get
            {
                return this;
            }
        }

        public bool IsSelected
        {
            get; set;
        }

        public AStarNode<Hex> OccupiedNode
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
            IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void Deselect()
        {
            GetComponent<Renderer>().material.color = Color.blue;
            IsSelected = false;
            Debug.Log("Deselected");
        }

        public void Attack()
        {

        }

        public void OnCursorEnter()
        {
            if (!IsSelected && IsSelectable)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
        }

        public void OnCursorExit()
        {
            if (!IsSelected && IsSelectable)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }
}