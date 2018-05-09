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
        protected Queue<NavNode<Hex>> m_Path;
        protected List<NavNode<Hex>> m_MoveableTiles;

        public Queue<NavNode<Hex>> Path { get { return m_Path; } }
        public List<NavNode<Hex>> MoveableTiles { get { return m_MoveableTiles; } }

        [SerializeField] private float m_DrawScale = 1.0f;

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
            m_Path = new Queue<NavNode<Hex>>();
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
                m_SelectedUnit.OnMoveNode -= UpdateUnitsMoveableTiles;
                OnDeselectUnit.Invoke(m_SelectedUnit);
                m_SelectedUnit.Deselect();
                m_SelectedUnit = null;
                m_TargetTile = null;
                m_MoveableTiles.Clear();
                m_Path.Clear();
            }
        }

        public void ConfirmOrder()
        {
            if (m_SelectedUnit != null && m_TargetTile != null)
            {
                Debug.Log("Confirmed Order");
                m_SelectedUnit.OnMoveNode += UpdateUnitsMoveableTiles;
                m_SelectedUnit.Move(m_Path, m_GameManager);
            }
        }

        public void CancelOrder()
        {

        }

        public void UpdateUnitsMoveableTiles()
        {
            m_MoveableTiles = m_GameManager.GetTilesInMovementRange(m_SelectedUnit.Position, m_SelectedUnit.MovementRange);
        }

        public void UpdateUnitsPath()
        {
            m_Path = new Queue<NavNode<Hex>>(m_GameManager.GetPath(m_SelectedUnit.Position, m_TargetTile.HexNode.Data));
        }

        public void TurnBegin()
        {
            //Debug.Log("MY TURN BEGAN");
        }

        public void TurnEnd()
        {
            //Debug.Log("MY TURN ENDED");
        }


        public void OnDrawGizmos()
        {
            if (m_IsMyTurn)
            {
                if (m_SelectedUnit != null)
                {
                    foreach (NavNode<Hex> node in m_Path)
                    {
                        m_GameManager.DrawHex(node.Data, Color.yellow, m_DrawScale);
                    }
                }

                foreach (NavNode<Hex> hexNode in m_MoveableTiles)
                {
                    m_GameManager.DrawHex(hexNode.Data, Color.green, Mathf.Lerp(0.5f, m_DrawScale - 0.1f, 0.5f * (Mathf.Sin(Time.time) + 1.0f)));
                }
            }
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
