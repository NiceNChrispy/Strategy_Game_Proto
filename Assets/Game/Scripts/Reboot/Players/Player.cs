using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField, Range(0, 20)] protected int m_MaxActionPoints;
        [SerializeField, Range(0, 20)] protected int m_ActionPoints;

        [SerializeField] protected List<Unit> m_Units;

        public event Action<Unit> OnSelectUnit = delegate { };
        public event Action<Unit> OnDeselectUnit = delegate { };
        public event Action OnTimerFinished = delegate { };
        public event Action OnTurnBegin = delegate { };
        public event Action OnTurnEnd = delegate { };

        [SerializeField] protected bool m_IsMyTurn;

        [SerializeField, ReadOnly] protected float m_RemainingTime;

        Coroutine m_CountdownRoutine;

        protected GameManager m_GameManager;

        [SerializeField] protected Unit m_SelectedUnit;
        public Unit SelectedUnit { get { return m_SelectedUnit; } private set { m_SelectedUnit = value; } }
        protected List<NavNode<Hex>> m_Path;
        protected List<NavNode<Hex>> m_MoveableTiles;

        public List<NavNode<Hex>> Path { get { return m_Path; } }
        public List<NavNode<Hex>> MoveableTiles { get { return m_MoveableTiles; } }

        public List<Unit> Units
        {
            get
            {
                return m_Units;
            }
        }

        protected Team m_Team;

        protected Tile m_TargetTile;

        public void Init(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_Team = Team.CreateNewTeam();
            //Debug.Log(m_Team.ID);
        }

        private void Awake()
        {
            m_ActionPoints = m_MaxActionPoints;
            m_MoveableTiles = new List<NavNode<Hex>>();
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
            //UpdateUnitsMoveableTiles();
            //Draw movement range
        }

        public void DeselectSelectedUnit()
        {
            if (m_SelectedUnit != null)
            {
                m_SelectedUnit.OnFinishMove -= UpdateUnitsMoveableTiles;
                OnDeselectUnit.Invoke(m_SelectedUnit);
                m_SelectedUnit.Deselect();
                m_SelectedUnit = null;
                m_MoveableTiles.Clear();
            }
        }

        public void ConfirmOrder()
        {

        }

        public void CancelOrder()
        {

        }

        public void UpdateUnitsMoveableTiles()
        {
            m_MoveableTiles = m_GameManager.GetTilesInMovementRange(m_SelectedUnit.Position, m_SelectedUnit.MovementRange);
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
            DeselectSelectedUnit();
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
