using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectableComponent<Unit>
    {
        [Section("Unit Info")]
        public string unitName;
        public Image characterImage;
        public int unitHealth;
        public int maxHealth = 10;
        [SerializeField, Range(0, 10)] private int m_MovementRange = 5;
        [SerializeField] private float m_MovementSpeed;
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
        public bool IsMoving { get; set; }

        public NavNode<Hex> OccupiedNode { get; set; }
        public Hex Position
        {
            get { return OccupiedNode.Data; }
        }

        public event Action OnFinishMove = delegate { };

        private void OnEnable()
        {
            IsSelectable = true;
            unitHealth = maxHealth;
        }

        public void Select()
        {
            IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void Deselect()
        {
            GetComponent<Renderer>().material.color = Color.white;
            IsSelected = false;
        }

        public void Attack()
        {

        }

        public void Move(List<NavNode<Hex>> path, GameManager gameManager)
        {
            StartCoroutine(MoveRoutine(path, gameManager));
        }

        public IEnumerator MoveRoutine(List<NavNode<Hex>> path, GameManager gameManager)
        {
            float journeyTime = path.Count / m_MovementSpeed;
            IsMoving = true;
            //while (journeyTime > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    OccupiedNode.IsTraversible = true;
                    OccupiedNode = path[i];
                    OccupiedNode.IsTraversible = false;
                    transform.position = gameManager.HexToWorld(path[i].Data);
                    journeyTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            OnFinishMove.Invoke();
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
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}