using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Reboot
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("Main UI")]
        [SerializeField] private Canvas m_UICanvas;
        [SerializeField] private GameObject m_UIPanel;
        [SerializeField] private GameObject m_AttackPanel;
        [SerializeField] private Player m_Player;
        [SerializeField] private Text m_TimerText;
        [SerializeField] private GameManager m_GameManager;
        [SerializeField] private LineRenderer m_PathLine;
        
        [Header("Side UI")]
        [SerializeField] private GameObject characterUIPrefab;
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject characterUIHolder;
        [SerializeField] private bool chracterDrawOpen;
        [SerializeField] private Image arrow;
        [SerializeField] private List<CharacterInfo> characterList;

        private void OnEnable()
        {
            m_Player.OnSelectUnit += OnPlayerSelectedUnit;
            m_Player.OnDeselectUnit += OnPlayerDeselectedUnit;
            m_UIPanel.SetActive(false);
        }

        public void OnPlayerSelectedUnit(Unit unit)
        {
            m_Player.OnPathChanged += UpdatePathLineRenderer;
            m_UIPanel.SetActive(true);
        }

        public void OnPlayerDeselectedUnit(Unit unit)
        {
            m_Player.OnPathChanged -= UpdatePathLineRenderer;
            m_UIPanel.SetActive(false);
            m_AttackPanel.SetActive(false);
        }

        private void UpdatePathLineRenderer()
        {
            m_PathLine.positionCount = m_Player.Path.Count + 1;
            int i = 0;
            m_PathLine.SetPosition(i++, m_GameManager.HexToWorld(m_Player.SelectedUnit.Position));
            foreach (INavNode<Hex> hexNode in m_Player.Path)
            {
                Vector3 position = m_GameManager.HexToWorld(hexNode.Position);
                position.z = -0.001f;
                m_PathLine.SetPosition(i++, position);
            }
        }

        private void Update()
        {
            m_TimerText.text = string.Format("{0:00.00}", m_GameManager.TimeBeforeNextPlayersTurn);
            //m_TimerText.color = Color.Lerp(Color.red, Color.black, Mathf.Sin(m_GameManager.TimeBeforeNextPlayersTurn * 5f));
        }

        public void LoadCharacterUI()
        {

        }
    }
}