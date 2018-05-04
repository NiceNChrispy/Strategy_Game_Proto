using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class Player : MonoBehaviour
    {
        [SerializeField, Range(0, 20)] private int m_MaxActionPoints;
        [SerializeField, Range(0, 20)] private int m_ActionPoints;

        [SerializeField] protected List<Unit> m_Units;

        protected event Action<Unit> OnSelectUnit = delegate { };
        public event Action OnTimerFinished = delegate { };
        public event Action OnTurnBegin = delegate { };
        public event Action OnTurnEnd = delegate { };

        [SerializeField] bool m_IsMyTurn;

        [SerializeField, ReadOnly] float m_RemainingTime;

        Coroutine m_CountdownRoutine;

        protected GameManager m_GameManager;

        [SerializeField] protected Unit m_SelectedUnit;
        public Unit SelectedUnit { get { return m_SelectedUnit; } private set { m_SelectedUnit = value; } }
        protected List<AStarNode<Hex>> m_Path;
        protected List<AStarNode<Hex>> m_MoveableTiles;

        public List<AStarNode<Hex>> Path { get { return m_Path; } }
        public List<AStarNode<Hex>> MoveableTiles { get { return m_MoveableTiles; } }

        public List<Unit> Units
        {
            get
            {
                return m_Units;
            }
        }

        public void Init(GameManager gameManager)
        {
            m_GameManager = gameManager;
        }

        private void Awake()
        {
            m_ActionPoints = m_MaxActionPoints;
        }

        private void OnEnable()
        {
            OnTurnBegin += TurnBegin;
            OnTurnEnd += TurnEnd;
        }

        private void OnDisable()
        {
            OnTurnBegin -= TurnBegin;
            OnTurnEnd -= TurnEnd;
        }

        protected void SelectUnit(Unit unit)
        {
            m_SelectedUnit = unit;
            m_SelectedUnit.Select();
            OnSelectUnit.Invoke(m_SelectedUnit);
            UpdateUnitsMoveableTiles();
            //Draw movement range
        }

        protected void DeselectUnit()
        {
            m_SelectedUnit.Deselect();
            m_SelectedUnit = null;
        }

        public void UpdateUnitsMoveableTiles()
        {
            m_MoveableTiles = m_GameManager.GetTilesInRange(m_SelectedUnit.Position, m_SelectedUnit.MovementRange);
        }

        public void TurnBegin()
        {
            //Debug.Log("MY TURN BEGAN");
        }

        public void TurnEnd()
        {
            //Debug.Log("MY TURN ENDED");
        }

        public void EndTurn()
        {
            if (m_CountdownRoutine != null)
            {
                StopCoroutine(m_CountdownRoutine);
                m_CountdownRoutine = null;
            }
            m_RemainingTime = 0f;
            m_IsMyTurn = false;
            OnTurnEnd.Invoke();
        }

        public IEnumerator CountDown(float turnLength, Action onComplete)
        {
            m_RemainingTime = turnLength;
            m_IsMyTurn = true;
            OnTurnBegin.Invoke();
            while (m_RemainingTime > 0)
            {
                m_RemainingTime -= Time.deltaTime;
                yield return null;
            }
            EndTurn();
            onComplete.Invoke();
        }
    }
}
