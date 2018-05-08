using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reboot
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Canvas m_UICanvas;
        [SerializeField] private GameObject m_UIPanel;
        [SerializeField] private Player m_Player;

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

        public void EnableOrderButtons()
        {

        }
    }
}