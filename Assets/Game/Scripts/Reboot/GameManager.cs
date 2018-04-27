using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class GameManager : MonoBehaviour
    {
        public List<Player> m_Players;
        public int m_PlayerTurn;
        public int m_StartingPlayerTurn;
        public Map m_Map;
        [SerializeField, Range(0, 180)] int m_TurnLength = 30;
        [SerializeField, ReadOnly] int m_TurnCount = 0;

        public Player PlayerWithTurn { get { return m_Players[m_PlayerTurn]; } }

        private void Awake()
        {
            Begin();
        }

        public void Begin()
        {
            m_PlayerTurn = Random.Range(0, m_Players.Count);
            m_StartingPlayerTurn = m_PlayerTurn;
            m_TurnCount = 0;
            StartCoroutine(PlayerWithTurn.CountDown((float)m_TurnLength, OnTurnComplete));
        }

        public void OnTurnComplete()
        {
            m_PlayerTurn = (m_PlayerTurn + 1) % m_Players.Count;
            if (m_PlayerTurn == m_StartingPlayerTurn)
            {
                m_TurnCount++;
            }

            StartCoroutine(PlayerWithTurn.CountDown((float)m_TurnLength, OnTurnComplete));
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha8))
            {
                PlayerWithTurn.EndTurn();
            }
        }
    }
}