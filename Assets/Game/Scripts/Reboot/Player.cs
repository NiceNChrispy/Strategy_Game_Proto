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

        public event Action<Unit> OnSelectUnit;
        public event Action OnTimerFinished = delegate { };
        public event Action OnTurnBegin = delegate { };
        public event Action OnTurnEnd = delegate { };

        [SerializeField] bool m_IsMyTurn;

        [SerializeField, ReadOnly] float m_RemainingTime;

        Coroutine m_CountdownRoutine;

        protected GameManager m_GameManager;

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
