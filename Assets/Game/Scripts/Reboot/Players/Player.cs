using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum OrderType { NONE = 0, MOVE = 1, ATTACK = 2 }
namespace Reboot
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField, Range(0, 20)] protected int m_ActionPoints;
        [SerializeField, Range(0, 20)] protected int m_MaxActionPoints;

        public int ActionPoints { get { return m_ActionPoints; } }
        public int MaxActionPoints { get { return m_MaxActionPoints; } }

        [SerializeField] protected List<Unit> m_Units;

        public event Action<Unit> OnSelectUnit = delegate { };
        public event Action<Unit> OnDeselectUnit = delegate { };
        public event Action OnTimerFinished = delegate { };
        public event Action OnTurnBegin = delegate { };
        public event Action OnTurnEnd = delegate { };

        [SerializeField] protected bool m_IsMyTurn;

        [SerializeField, ReadOnly] protected float m_RemainingTime;

        Coroutine m_CountdownRoutine;

        [SerializeField] protected Unit m_SelectedUnit;
        public Unit SelectedUnit { get { return m_SelectedUnit; } private set { m_SelectedUnit = value; } }
        protected Queue<Tile> m_Path;
        protected List<Tile> m_MoveableTiles;
        protected List<Tile> m_AttackableTiles;

        public Queue<Tile> Path { get { return m_Path; } }
        public List<Tile> MoveableTiles { get { return m_MoveableTiles; } }
        public List<Tile> AttackableTiles { get { return m_AttackableTiles; } }

        [SerializeField] private OrderType m_CurrentOrder;

        public List<Unit> Units
        {
            get
            {
                return m_Units;
            }
        }

        [SerializeField] protected Tile m_TargetTile;

        protected int m_CurrentAttackIndex = -1;

        public float RemainingTime { get { return m_RemainingTime; } }

        public OrderType CurrentOrder
        {
            get
            {
                return m_CurrentOrder;
            }
        }

        [SerializeField] protected bool m_IsBusy;

        public event Action OnPathChanged = delegate { };
        public event Action OnTilesUpdated = delegate { };

        protected bool IsMyUnit(Unit unit)
        {
            return m_Units.Contains(unit);
        }

        public void Init()
        {
            m_ActionPoints = m_MaxActionPoints;
            m_MoveableTiles = new List<Tile>();
            m_AttackableTiles = new List<Tile>();
            m_Path = new Queue<Tile>();
            m_Units = GetComponentsInChildren<Unit>().ToList();
            OnTilesUpdated.Invoke();
        }

        protected void TargetTile(Tile tile)
        {
            m_TargetTile = tile;
            if (m_MoveableTiles.Contains(m_TargetTile))
            {
                UpdateUnitsPath();
            }
            if (m_AttackableTiles.Contains(m_TargetTile))
            {

            }
        }

        protected void SelectUnit(Unit unit)
        {
            m_SelectedUnit = unit;
            m_SelectedUnit.Select();
            OnSelectUnit.Invoke(m_SelectedUnit);
        }

        public void DeselectSelectedUnit()
        {
            if (m_SelectedUnit != null)
            {
                OnDeselectUnit.Invoke(m_SelectedUnit);
                m_SelectedUnit.Deselect();
                m_SelectedUnit = null;
            }
            ResetEverything();
        }

        public void ResetEverything()
        {
            m_MoveableTiles.Clear();
            m_AttackableTiles.Clear();
            m_CurrentAttackIndex = -1;
            OnTilesUpdated.Invoke();
            //m_Path.Clear();
            m_TargetTile = null;
        }

        //BUG: Ok so weird bug if you click confirm and there is a hex behind the button then the path destination is changed last minute.
        public void ConfirmOrder()
        {
            if (m_SelectedUnit != null && m_TargetTile != null)
            {
                Debug.Log(string.Format("Confirmed Order: {0}", m_CurrentOrder));
                switch (m_CurrentOrder)
                {
                    case OrderType.NONE:
                    break;
                    case OrderType.MOVE:
                    {
                        m_SelectedUnit.Move(m_Path, GameManager.Instance, OnUnitMove, OnUnitFinishedAction);
                        m_IsBusy = true;
                        break;
                    }
                    case OrderType.ATTACK:
                    {
                        GameManager.Instance.Attack(m_TargetTile, m_SelectedUnit.Attacks[m_CurrentAttackIndex].Data);

                        AttackEffect effect = Instantiate(m_SelectedUnit.Attacks[m_CurrentAttackIndex].Effect);
                        effect.transform.position = GameManager.Instance.HexToWorld(m_TargetTile.Position);
                        effect.Play();
                        m_IsBusy = true;
                        break;
                    }
                    default:
                    break;
                }
            }
        }

        protected void OnUnitFinishedAction()
        {
            m_IsBusy = false;
            if (m_ActionPoints == 0)
            {
                EndTurn();
            }
        }

        //Hack to set order using button
        public void SetOrder_BUTTON(int orderID)
        {
            SetOrder((OrderType)orderID);
        }

        public void SetOrder(OrderType order)
        {
            Debug.Log("ORDER SET TO: " + order);
            ResetEverything();
            m_CurrentOrder = order;
            switch (m_CurrentOrder)
            {
                case OrderType.NONE:
                {
                    DeselectSelectedUnit();
                    break;
                }
                case OrderType.MOVE:
                {
                    UpdateMoveableTiles();
                    break;
                }
                case OrderType.ATTACK:
                {
                    UpdateAttackableTiles();
                    break;
                }
                default:
                break;
            }
        }

        private void UpdateMoveableTiles()
        {
            m_MoveableTiles = GameManager.Instance.GetTilesInMovementRange(m_SelectedUnit.Position, Mathf.Min(m_SelectedUnit.MovementRange, m_ActionPoints));
            OnTilesUpdated();
        }

        private void UpdateAttackableTiles()
        {
            if (m_CurrentAttackIndex != -1)
            {
                m_AttackableTiles = GameManager.Instance.GetTilesInAttackRange(m_SelectedUnit.Position, Mathf.Min(m_SelectedUnit.MovementRange, m_ActionPoints));
            }
            OnTilesUpdated();
        }

        public void OnUnitMove()
        {
            m_ActionPoints--;
            UpdateMoveableTiles();
            OnPathChanged.Invoke();
        }

        public void SetAttackIndex(int index)
        {
            m_CurrentAttackIndex = index;
            UpdateAttackableTiles();
        }

        public void UpdateUnitsPath()
        {
            m_Path = new Queue<Tile>(GameManager.Instance.GetPath(m_SelectedUnit.Position, m_TargetTile.Position));
            OnPathChanged.Invoke();
        }

        public void EndTurn()
        {
            if (m_CountdownRoutine != null)
            {
                StopCoroutine(m_CountdownRoutine);
                m_CountdownRoutine = null;
            }
            m_RemainingTime = 0f;
            m_CurrentOrder = OrderType.NONE;
            m_IsMyTurn = false;
            DeselectSelectedUnit();
            OnTilesUpdated.Invoke();
        }

        public IEnumerator CountDown(float turnLength, Action onComplete)
        {
            m_RemainingTime = turnLength;
            m_IsMyTurn = true;
            m_ActionPoints = m_MaxActionPoints;
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
