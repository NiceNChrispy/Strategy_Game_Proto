using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Reboot
{
    public class Unit : MonoBehaviour, ISelectableComponent<Unit>, IHealth<Unit>
    {
        [Header("Unit Info")]
        public string unitName;
        public Image characterImage;
        [SerializeField, Range(0, 10)] private int m_MovementRange = 5;
        [SerializeField] private float m_MovementSpeed;

        [SerializeField] private AttackBehaviour[] m_Attacks;

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

        public INavNode<Hex> OccupiedNode { get; set; }
        public Hex Position
        {
            get { return OccupiedNode.Position; }
        }

        public AttackBehaviour[] Attacks
        {
            get
            {
                return m_Attacks;
            }
        }

        [SerializeField] private int m_Health = 10;
        [SerializeField] private int m_MaxHealth = 10;

        public int Health { get { return m_Health; } set { m_Health = value; } }
        public int MaxHealth { get { return m_MaxHealth; } set { m_MaxHealth = value; } }

        private void OnEnable()
        {
            IsSelectable = true;
            Health = MaxHealth;
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

        public void Attack(INavNode<Hex> targetNode, GameManager gameManager)
        {
        }

        public void Move(Queue<Tile> path, GameManager gameManager, Action OnMoveCallback, Action OnCompleteMoveCallback)
        {
            if((INavNode<Hex>)path.Peek() == OccupiedNode)
            {
                path.Dequeue();
            }
            StartCoroutine(MoveRoutine(path, gameManager,OnMoveCallback, OnCompleteMoveCallback));
        }

        public IEnumerator MoveRoutine(Queue<Tile> path, GameManager gameManager, Action OnMoveCallback, Action OnCompleteMoveCallback)
        {
            float journeyTime = path.Count / m_MovementSpeed;
            IsMoving = true;
            while (path.Count > 0)
            {
                OccupiedNode.IsTraversible = true;
                OccupiedNode = path.Dequeue();
                OccupiedNode.IsTraversible = false;
                transform.position = gameManager.HexToWorld(OccupiedNode.Position);
                journeyTime -= Time.deltaTime;
                OnMoveCallback.Invoke();
                yield return new WaitForSeconds(0.5f);
            }
            OnCompleteMoveCallback.Invoke();
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

        public void Heal()
        {
            throw new NotImplementedException();
        }

        public void Heal(int amount)
        {
            throw new NotImplementedException();
        }

        public void Damage(int amount)
        {
            if (amount > 0)
            {
                m_Health = Mathf.Clamp(m_Health - amount, 0, m_MaxHealth);
            }
            if (m_Health == 0)
            {
                Kill();
            }
        }

        public void Kill()
        {
            Debug.Log("I AM DEAD");
        }
    }
}