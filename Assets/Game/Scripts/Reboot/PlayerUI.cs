using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reboot
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Canvas m_UICanvas;
        [SerializeField] private GameObject m_UIPanel;
        [SerializeField] private Player m_Player;
        [SerializeField] private Text m_TimerText;
        [SerializeField] private GameManager m_GameManager;

        private void OnEnable()
        {
            m_Player.OnSelectUnit += OnPlayerSelectedUnit;
            m_Player.OnDeselectUnit += OnPlayerDeselectedUnit;
            m_UIPanel.SetActive(false);
        }

        public void OnPlayerSelectedUnit(Unit unit)
        {
            m_UIPanel.SetActive(true);
        }

        public void OnPlayerDeselectedUnit(Unit unit)
        {
            m_UIPanel.SetActive(false);
        }

        private void Update()
        {
            m_TimerText.text = string.Format("{0:00.00}", m_GameManager.TimeBeforeNextPlayersTurn);
            //m_TimerText.color = Color.Lerp(Color.red, Color.black, Mathf.Sin(m_GameManager.TimeBeforeNextPlayersTurn * 5f));
        }
    }
}