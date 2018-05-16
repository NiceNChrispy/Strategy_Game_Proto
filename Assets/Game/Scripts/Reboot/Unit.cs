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

        public Unit Data
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
        public event Action OnMoveNode = delegate { };

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

        public void Attack(NavNode<Hex> targetNode, GameManager gameManager)
        {
        }

        public void Move(Queue<NavNode<Hex>> path, GameManager gameManager)
        {
            StartCoroutine(MoveRoutine(path, gameManager));
        }

        public IEnumerator MoveRoutine(Queue<NavNode<Hex>> path, GameManager gameManager)
        {
            float journeyTime = path.Count / m_MovementSpeed;
            IsMoving = true;
            //while (journeyTime > 0)
            {
                while(path.Count > 0)
                {
                    OccupiedNode.IsTraversible = true;
                    OccupiedNode = path.Dequeue();
                    OccupiedNode.IsTraversible = false;
                    transform.position = gameManager.HexToWorld(OccupiedNode.Data);
                    journeyTime -= Time.deltaTime;
                    OnMoveNode.Invoke();
                    yield return new WaitForSeconds(0.1f);
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