using DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectableComponent<Unit>
    {
        [SerializeField, Range(0, 10)] private int m_MovementRange = 5;
        [SerializeField]                private float m_MovementSpeed;
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

        public Unit SelectableComponent
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

        public Hex Position
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

        public void Move(List<AStarNode<Hex>> path, Layout layout)
        {
            StartCoroutine(MoveRoutine(path, layout));
        }

        public IEnumerator MoveRoutine(List<AStarNode<Hex>> path, Layout layout)
        {
            float journeyTime = path.Count / m_MovementSpeed;

            //while (journeyTime > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    transform.position = layout.HexToPixel(path[i].Data);
                    journeyTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
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