using DataStructures;
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

        protected GameManager m_GameManager;

        [SerializeField] protected Unit m_SelectedUnit;
        public Unit SelectedUnit { get { return m_SelectedUnit; } private set { m_SelectedUnit = value; } }
        protected Queue<NavNode<Hex>> m_Path;
        protected List<Tile> m_MoveableTiles;
        protected List<NavNode<Hex>> m_AttackableNodes;

        public Queue<NavNode<Hex>> Path { get { return m_Path; } }
        public List<Tile> MoveableTiles { get { return m_MoveableTiles; } }
        public List<NavNode<Hex>> AttackableNodes { get { return m_AttackableNodes; } }

        [SerializeField] private float m_DrawScale = 1.0f;
        [SerializeField] private OrderType m_CurrentOrder;

        public List<Unit> Units
        {
            get
            {
                return m_Units;
            }
        }

        protected Team m_Team;

        [SerializeField] protected NavNode<Hex> m_TargetNode;

        protected int m_CurrentAttackIndex = -1;

        public float RemainingTime { get { return m_RemainingTime; } }

        public event Action<Tile> OnTargetNewTile;

        [SerializeField] private LineRenderer m_PathLine;

        public void Init(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_Team = Team.CreateNewTeam();

            m_ActionPoints = m_MaxActionPoints;
            m_MoveableTiles = new List<Tile>();
            m_AttackableNodes = new List<NavNode<Hex>>();
            m_Path = new Queue<NavNode<Hex>>();
            m_Units = GetComponentsInChildren<Unit>().ToList();
            //Debug.Log(m_Team.ID);
        }

        protected void TargetTile(Tile tile)
        {
            Debug.Log("TARGETED");
            m_TargetNode = tile.HexNode;
            if (m_MoveableTiles.Contains(tile))
            {
                UpdateUnitsPath();
            }
            if (m_AttackableNodes.Contains(m_TargetNode))
            {

            }
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
                //m_SelectedUnit.OnFinishMove -= ClearAllUnitInfo;
                OnDeselectUnit.Invoke(m_SelectedUnit);
                m_SelectedUnit.Deselect();
                m_SelectedUnit = null;
            }
            ClearAllUnitInfo();
        }

        public void ClearAllUnitInfo()
        {
            foreach (Tile tile in m_MoveableTiles)
            {
                tile.SetState(Tile.State.NONE);
            }
            m_MoveableTiles.Clear();
            m_AttackableNodes.Clear();
            m_CurrentAttackIndex = -1;
            //m_Path.Clear();
            m_TargetNode = null;
        }

        //BUG: Ok so weird bug if you click confirm and there is a hex behind the button then the path destination is changed last minute.
        public void ConfirmOrder()
        {
            if (m_SelectedUnit != null && m_TargetNode != null)
            {
                Debug.Log(string.Format("Confirmed Order: {0}", m_CurrentOrder));
                switch (m_CurrentOrder)
                {
                    case OrderType.NONE:
                    break;
                    case OrderType.MOVE:
                    {
                        m_SelectedUnit.Move(m_Path, m_GameManager, OnUnitMove, OnUnitFinishedAction);
                        break;
                    }
                    case OrderType.ATTACK:
                    {
                        bool attackSuccessful = m_GameManager.Attack(m_TargetNode, m_SelectedUnit.Attacks[m_CurrentAttackIndex].Data);

                        if (attackSuccessful)
                        {
                            AttackEffect effect = Instantiate(m_SelectedUnit.Attacks[m_CurrentAttackIndex].Effect);
                            effect.transform.position = m_GameManager.HexToWorld(m_TargetNode.Data);
                            effect.Play();
                        }

                        Debug.Log(string.Format("Attack {0}", attackSuccessful ? "SUCCEEDED" : "FAILED"));
                        break;
                    }
                    default:
                    break;
                }
            }
        }

        protected virtual void OnUnitFinishedAction()
        {
            Debug.Log("Finished Action");
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
            ClearAllUnitInfo();
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
                    UpdateMoveableNodes();
                    break;
                }
                case OrderType.ATTACK:
                {
                    if (m_CurrentAttackIndex != -1)
                    {
                        m_AttackableNodes = m_GameManager.GetNodesInRange(m_SelectedUnit.Position, m_SelectedUnit.Attacks[m_CurrentAttackIndex].Range);
                    }
                    break;
                }
                default:
                break;
            }
        }

        private void UpdateMoveableNodes()
        {
            foreach (Tile tile in m_MoveableTiles)
            {
                tile.SetState(Tile.State.NONE);
            }

            m_MoveableTiles = m_GameManager.GetTilesInRange(m_SelectedUnit.Position, Mathf.Min(m_SelectedUnit.MovementRange, m_ActionPoints));

            foreach (Tile tile in m_MoveableTiles)
            {
                tile.SetState(Tile.State.MOVEABLE);
            }
        }

        public void OnUnitMove()
        {
            m_ActionPoints--;
            UpdatePathLineRenderer();
            UpdateMoveableNodes();
        }

        public void SetAttackIndex(int index)
        {
            m_CurrentAttackIndex = index;
            if (m_CurrentAttackIndex != -1)
            {
                m_AttackableNodes = m_GameManager.GetNodesInRange(m_SelectedUnit.Position, m_SelectedUnit.Attacks[m_CurrentAttackIndex].Range);
            }
        }

        public void UpdateUnitsPath()
        {
            m_Path = new Queue<NavNode<Hex>>(m_GameManager.GetPath(m_SelectedUnit.Position, m_TargetNode.Data));
            UpdatePathLineRenderer();
        }

        private void UpdatePathLineRenderer()
        {
            m_PathLine.positionCount = m_Path.Count + 1;
            int i = 0;
            m_PathLine.SetPosition(i++, m_GameManager.HexToWorld(m_SelectedUnit.Position));
            foreach (NavNode<Hex> hexNode in m_Path)
            {
                m_PathLine.SetPosition(i++, m_GameManager.HexToWorld(hexNode.Data));
            }
        }

        public virtual void TurnBegin()
        {
            //Debug.Log("MY TURN BEGAN");
        }

        public virtual void TurnEnd()
        {
            //Debug.Log("MY TURN ENDED");
        }

        void DrawHexes()
        {
            if (m_IsMyTurn)
            {
                switch (m_CurrentOrder)
                {
                    case OrderType.NONE:
                    break;
                    case OrderType.MOVE:
                    {
                        if (m_SelectedUnit != null)
                        {
                            foreach (NavNode<Hex> node in m_Path)
                            {
                                m_GameManager.DrawHex(node.Data, Color.yellow, m_DrawScale);
                            }
                        }
                        foreach (Tile tile in m_MoveableTiles)
                        {
                            m_GameManager.DrawHex(tile.HexNode.Data, Color.green, Mathf.Lerp(0.5f, m_DrawScale - 0.1f, 0.5f * (Mathf.Sin(Time.time) + 1.0f)));
                        }
                        break;
                    }
                    case OrderType.ATTACK:
                    {
                        foreach (NavNode<Hex> hexNode in m_AttackableNodes)
                        {
                            m_GameManager.DrawHex(hexNode.Data, Color.red, Mathf.Lerp(0.5f, m_DrawScale - 0.1f, 0.5f * (Mathf.Sin(Time.time) + 1.0f)));
                        }
                    }
                    break;
                    default:
                    break;
                }

            }
        }

        public void OnDrawGizmos()
        {
            DrawHexes();
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
            m_ActionPoints = m_MaxActionPoints;
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
